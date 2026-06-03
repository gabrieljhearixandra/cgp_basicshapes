import turtle
import tkinter as tk
from tkinter import filedialog, messagebox, colorchooser
from tkinter import font as tkfont
import os
import random
import time
import threading
import json
import sys

# ── Custom Font Setup ─────────────────────────────────────────────────────────
FONT_PATH = os.path.join(os.path.dirname(os.path.abspath(__file__)), "PressStart2P-Regular.ttf")

def register_font():
    """Register Press Start 2P font with the system temporarily."""
    if sys.platform == "win32":
        try:
            import ctypes
            from ctypes import wintypes
            # Load font into Windows registry temporarily (process lifetime)
            FR_PRIVATE = 0x10
            gdi32 = ctypes.WinDLL('gdi32', use_last_error=True)
            if os.path.exists(FONT_PATH):
                result = gdi32.AddFontResourceExW(FONT_PATH, FR_PRIVATE, 0)
                if result:
                    return "Press Start 2P"
        except Exception:
            pass
    return "Courier"  # fallback

# Register font
CUSTOM_FONT = register_font()

# ── Sound (pygame.mixer preferred; winsound fallback) ────────────────────────
# =============================================================================
#  BACKGROUND MUSIC  –  HOW TO ADD YOUR OWN MUSIC
# =============================================================================
#  Place these WAV files in the SAME folder as this script:
#
#    lobby_music.wav  – loops while the lobby is open
#    game_music.wav   – loops during any game match
#    win_music.wav    – plays once when a player wins
#    Boing.wav        – plays when the ball hits a paddle
#    miss.wav         – plays when the ball passes a paddle (miss / point s-*-*--*cored)
#    powerup.wav      – plays when a power-up is collected
#
#  Use the 🔊 Sound ON / 🔇 Sound OFF button in the lobby to mute everything.
#  WAV format: 16-bit PCM, 44100 Hz, stereo or mono.
# =============================================================================

# Try pygame first (proper multi-channel audio)

try:
    import pygame
    pygame.mixer.init(frequency=44100, size=-16, channels=2, buffer=512)
    HAS_PYGAME = True
except Exception:
    HAS_PYGAME = False

# Fallback: winsound (Windows only, single channel – music only, no SFX overlap)
if not HAS_PYGAME:
    try:
        import winsound
        HAS_WINSOUND = True
    except ImportError:
        HAS_WINSOUND = False
else:
    HAS_WINSOUND = False

# Global sound toggle (True = sound on)
sound_enabled = True

# ── pygame path ───────────────────────────────────────────────────────────────
_SCRIPT_DIR = os.path.dirname(os.path.abspath(__file__))

def _abspath(filename):
    """Resolve a filename relative to the script directory."""
    if os.path.isabs(filename):
        return filename
    return os.path.join(_SCRIPT_DIR, filename)

# Pre-loaded SFX cache (pygame Sound objects)
_sfx_cache = {}

def _load_sfx(filename):
    """Load and cache a pygame Sound object."""
    path = _abspath(filename)
    if path not in _sfx_cache:
        if os.path.exists(path):
            try:
                _sfx_cache[path] = pygame.mixer.Sound(path)
            except Exception:
                _sfx_cache[path] = None
        else:
            _sfx_cache[path] = None
    return _sfx_cache[path]

# ── Music (pygame) ────────────────────────────────────────────────────────────
def start_music(filename):
    """Start looping a music file. Stops any currently playing music first."""
    if not sound_enabled:
        return
    path = _abspath(filename)
    if HAS_PYGAME:
        try:
            if not os.path.exists(path):
                return
            pygame.mixer.music.stop()
            pygame.mixer.music.load(path)
            pygame.mixer.music.play(-1)   # -1 = loop forever
        except Exception:
            pass
    elif HAS_WINSOUND:
        stop_music()
        if not os.path.exists(path):
            return
        global _ws_stop_flag, _ws_thread
        _ws_stop_flag = False
        _ws_thread = threading.Thread(target=_ws_music_loop, args=(path,), daemon=True)
        _ws_thread.start()

def stop_music():
    """Stop music immediately."""
    if HAS_PYGAME:
        try:
            pygame.mixer.music.stop()
        except Exception:
            pass
    elif HAS_WINSOUND:
        global _ws_stop_flag
        _ws_stop_flag = True
        try:
            winsound.PlaySound(None, winsound.SND_ASYNC)
        except Exception:
            pass

def stop_all_sound():
    """Stop music AND all SFX channels (used by mute)."""
    stop_music()
    if HAS_PYGAME:
        try:
            pygame.mixer.stop()   # stops all non-music channels
        except Exception:
            pass
    elif HAS_WINSOUND:
        try:
            winsound.PlaySound(None, winsound.SND_ASYNC)
        except Exception:
            pass

# ── SFX (pygame) ──────────────────────────────────────────────────────────────
def play_sfx(filename):
    """Play a one-shot SFX without interrupting music."""
    if not sound_enabled:
        return
    if HAS_PYGAME:
        snd = _load_sfx(filename)
        if snd:
            try:
                snd.play()
            except Exception:
                pass
    elif HAS_WINSOUND:
        path = _abspath(filename)
        try:
            if os.path.exists(path):
                winsound.PlaySound(path, winsound.SND_ASYNC | winsound.SND_FILENAME)
        except Exception:
            pass

def play_sfx_blocking(filename):
    """Play a SFX and wait for it to finish (used for win jingle)."""
    if not sound_enabled:
        return
    if HAS_PYGAME:
        snd = _load_sfx(filename)
        if snd:
            try:
                ch = snd.play()
                if ch:
                    while ch.get_busy():
                        time.sleep(0.05)
            except Exception:
                pass
    elif HAS_WINSOUND:
        path = _abspath(filename)
        def _play():
            try:
                if os.path.exists(path):
                    winsound.PlaySound(path, winsound.SND_FILENAME)
            except Exception:
                pass
        t = threading.Thread(target=_play, daemon=True)
        t.start()
        t.join(timeout=10)

# ── winsound fallback music loop ──────────────────────────────────────────────
_ws_stop_flag = False
_ws_thread    = None

def _ws_music_loop(path):
    global _ws_stop_flag
    while not _ws_stop_flag:
        if not os.path.exists(path):
            break
        try:
            winsound.PlaySound(path, winsound.SND_FILENAME)
        except Exception:
            break


# ── Background image (stretch with Pillow if available) ──────────────────────
WIN_W, WIN_H = 800, 600
TEMP_BG      = "_stretched_bg_temp.gif"
CONFIG_FILE  = "pingpong_config.json"

def load_config():
    """Load saved background path from config file."""
    if os.path.exists(CONFIG_FILE):
        try:
            with open(CONFIG_FILE, 'r') as f:
                return json.load(f)
        except Exception:
            pass
    return {}

def save_config(config):
    """Save config to file."""
    try:
        with open(CONFIG_FILE, 'w') as f:
            json.dump(config, f, indent=2)
    except Exception:
        pass

def prepare_background(path):
    if not path or not os.path.exists(path):
        return None
    try:
        from PIL import Image
        img = Image.open(path).convert("RGB").resize((WIN_W, WIN_H), Image.LANCZOS)
        img.save(TEMP_BG, "GIF")
        return TEMP_BG
    except Exception:
        if path.lower().endswith(".gif"):
            return path
        return None


# ─────────────────────────────────────────────────────────────────────────────
#  LOBBY
# ─────────────────────────────────────────────────────────────────────────────
# Tooltip class for hover text
class ToolTip:
    def __init__(self, widget, text):
        self.widget = widget
        self.text = text
        self.tip_window = None
        widget.bind("<Enter>", self.show_tip)
        widget.bind("<Leave>", self.hide_tip)
    
    def show_tip(self, event=None):
        if self.tip_window or not self.text:
            return
        x = self.widget.winfo_rootx() + self.widget.winfo_width() // 2
        y = self.widget.winfo_rooty() - 30
        self.tip_window = tw = tk.Toplevel(self.widget)
        tw.wm_overrideredirect(True)
        tw.wm_geometry(f"+{x}+{y}")
        label = tk.Label(tw, text=self.text, justify='center',
                        background="#ffffe0", relief='solid', borderwidth=1,
                        font=(CUSTOM_FONT, 7))
        label.pack()
    
    def hide_tip(self, event=None):
        if self.tip_window:
            self.tip_window.destroy()
            self.tip_window = None

lobby = tk.Tk()
lobby.title("Ping Pong Game - Lobby")
lobby.geometry("460x530")
lobby.resizable(False, False)

# Load saved config
config = load_config()

# Apply saved lobby background color if exists
lobby_bg_color = config.get("lobby_bg", "#1a1a2e")
lobby.configure(bg=lobby_bg_color)

game_mode         = tk.StringVar(value="pvp")
play_mode         = tk.StringVar(value="normal")
bg_image_path_var = tk.StringVar(value=config.get("background", ""))
color_a           = tk.StringVar(value="cyan")
color_b           = tk.StringVar(value="red")

BG  = lobby_bg_color  # use the loaded/saved color
LBL = dict(bg=BG, fg="#e0e0ff", font=(CUSTOM_FONT, 8))
TTL = dict(bg=BG, fg="#ffffff",  font=(CUSTOM_FONT, 12, "bold"))
RDO = dict(bg=BG, fg="#ccccff", selectcolor="#16213e",
           activebackground=BG, font=(CUSTOM_FONT, 8))
BTN = dict(bg="#16213e", fg="white", font=(CUSTOM_FONT, 8), relief="flat")

# ── Help / tutorial text ──────────────────────────────────────────────────────
HELP_TEXT = """\
HOW TO PLAY
==================================================

OBJECTIVE
  Outscore your opponent to win!
  The target score depends on which mode you pick.

CONTROLS
  Player A  (left paddle)
    W  = move up        S  = move down

  Player B  (right paddle)  [PvP only]
    Up Arrow = move up   Down Arrow = move down

SCORING
  Ball passes your opponent's paddle = you get a point!

==================================================
GAME MODES
==================================================

NORMAL
  Classic Ping Pong.
  First to 10 points wins. Pure skill, no gimmicks.

POWER-UPS
  Same as Normal (first to 10 points) PLUS:
  A glowing white bubble spawns on the field every
  8-15 seconds. When the BALL hits it, the last
  player to touch the ball gets a random power-up:

    Long Paddle      Your paddle grows +50% for 4s
    Speed Boost      Your next hit fires ball at 1.6x speed
    Shrink Opponent  Opponent paddle shrinks -40% for 4s
    Ball Slowdown    Ball speed halved for 5s (both players)
    Double Points    Your next score counts as 2 points
    Invisible Paddle Your paddle hides for 3s (still deflects!)

  Bubbles disappear after 6s if not collected.
  A new one spawns 5-10s later. Only one at a time.

QUICK MATCH
  SUDDEN DEATH - one life each!
  The first player to let the ball past their paddle
  LOSES the whole match immediately.

  The ball also speeds up every 10 seconds of
  a running rally, shown by a colour flash and a
  "Speed x..." message on screen. There are up to
  8 speed tiers, so no rally can drag on forever!

==================================================
TIPS
  In Power-Ups mode, aim the ball at the bubble on
  purpose to grab a power-up for yourself!

  In Quick Match, one mistake ends it all - stay
  focused and don't chase the ball too aggressively.

  Watch the HUD labels on-screen to see which effects
  are currently active and how long they last.

  In Bot mode, the AI also benefits from all power-ups!

==================================================
SOUND
  Drop WAV files in the same folder as this script:
    Boing.wav      – paddle hit sound
    miss.wav       – ball missed / point scored sound
    powerup.wav    – power-up collected sound
    win_music.wav  – victory jingle
    game_music.wav – in-game background music
    lobby_music.wav– lobby background music

  Use the 🔊 Sound ON button in the lobby to mute
  or unmute all sounds and music at any time.
==================================================
"""

def show_help():
    win = tk.Toplevel(lobby)
    win.title("How to Play")
    win.configure(bg="#0f0f1e")
    win.resizable(False, False)
    frame = tk.Frame(win, bg="#0f0f1e")
    frame.pack(fill="both", expand=True, padx=8, pady=8)
    sb = tk.Scrollbar(frame)
    sb.pack(side="right", fill="y")
    txt = tk.Text(frame, bg="#0f0f1e", fg="#e8e8ff",
                  font=("Courier", 10), width=54, height=32,
                  relief="flat", padx=14, pady=10, wrap="word",
                  yscrollcommand=sb.set)
    txt.insert("1.0", HELP_TEXT)
    txt.config(state="disabled")
    txt.pack(side="left", fill="both", expand=True)
    sb.config(command=txt.yview)
    tk.Button(win, text="  Close  ", command=win.destroy,
              bg="#e94560", fg="white", font=("Arial", 10, "bold"),
              relief="flat").pack(pady=8)

# ── Lobby helpers ─────────────────────────────────────────────────────────────
def choose_bg_image():
    fp = filedialog.askopenfilename(
        title="Select background image (GIF, PNG, JPG)",
        filetypes=[("Image files", "*.gif *.png *.jpg *.jpeg"), ("All", "*.*")]
    )
    if fp:
        bg_image_path_var.set(fp)
        # Save to config
        config["background"] = fp
        save_config(config)
        messagebox.showinfo("Background Saved", "Your background choice has been saved!")
    else:
        pass

def _swatch(parent, var):
    s = tk.Label(parent, bg=var.get(), width=3, height=1, relief="sunken")
    def _refresh(*_):
        try: s.config(bg=var.get())
        except Exception: pass
    var.trace_add("write", _refresh)
    return s

def pick_color_a():
    c = colorchooser.askcolor(title="Player A paddle colour", color=color_a.get())
    if c and c[1]: color_a.set(c[1])

def pick_color_b():
    c = colorchooser.askcolor(title="Player B paddle colour", color=color_b.get())
    if c and c[1]: color_b.set(c[1])

def on_opponent_change(*_):
    if game_mode.get() == "pvp":
        row_color_b.pack(after=row_color_a, pady=2)
    else:
        row_color_b.pack_forget()

# ── Mute / Unmute toggle ──────────────────────────────────────────────────────
def toggle_sound():
    global sound_enabled
    sound_enabled = not sound_enabled
    if sound_enabled:
        btn_mute.config(text="🔊")
        start_music("lobby_music.wav")
    else:
        btn_mute.config(text="🔇")
        stop_all_sound()

def start_game():
    stop_music()
    bg = bg_image_path_var.get() or None
    ca = color_a.get()
    cb = color_b.get() if game_mode.get() == "pvp" else "#ff4444"
    gm = game_mode.get()
    pm = play_mode.get()
    run_game(gm, pm, bg, ca, cb, lobby)

# ── Layout ────────────────────────────────────────────────────────────────────
header_frame = tk.Frame(lobby, bg=BG)
header_frame.pack(fill="x", padx=14, pady=(12, 4))

# Center title
tk.Label(header_frame, text="Ping Pong Setup", **TTL).pack(expand=True)

tk.Label(lobby, text="Select Opponent:", **LBL).pack(pady=(8, 0))
tk.Radiobutton(lobby, text="Player vs Player", variable=game_mode, value="pvp",
               command=on_opponent_change, **RDO).pack()
tk.Radiobutton(lobby, text="Player vs Bot",    variable=game_mode, value="pvb",
               command=on_opponent_change, **RDO).pack()

tk.Label(lobby, text="Game Mode:", **LBL).pack(pady=(10, 0))
tk.Radiobutton(lobby, text="Normal",      variable=play_mode, value="normal",     **RDO).pack()
tk.Radiobutton(lobby, text="Power-Ups!",  variable=play_mode, value="powerups",   **RDO).pack()
tk.Radiobutton(lobby, text="Quick Match", variable=play_mode, value="quickmatch", **RDO).pack()

tk.Label(lobby, text="Paddle Colours:", **LBL).pack(pady=(10, 0))

row_color_a = tk.Frame(lobby, bg=BG)
row_color_a.pack(pady=2)
tk.Label(row_color_a, text="Player A:", **LBL).pack(side="left")
_swatch(row_color_a, color_a).pack(side="left", padx=4)
btn_color_a = tk.Button(row_color_a, text="Pick colour", command=pick_color_a, **BTN)
btn_color_a.pack(side="left")
btn_color_a.bind("<Enter>", lambda e: e.widget.config(bg="#2a3a5e"))
btn_color_a.bind("<Leave>", lambda e: e.widget.config(bg="#16213e"))

row_color_b = tk.Frame(lobby, bg=BG)
row_color_b.pack(pady=2)
tk.Label(row_color_b, text="Player B:", **LBL).pack(side="left")
_swatch(row_color_b, color_b).pack(side="left", padx=4)
btn_color_b = tk.Button(row_color_b, text="Pick colour", command=pick_color_b, **BTN)
btn_color_b.pack(side="left")
btn_color_b.bind("<Enter>", lambda e: e.widget.config(bg="#2a3a5e"))
btn_color_b.bind("<Leave>", lambda e: e.widget.config(bg="#16213e"))

btn_start = tk.Button(lobby, text="START GAME", command=start_game,
                      bg="#e94560", fg="white", font=(CUSTOM_FONT, 10, "bold"),
                      relief="flat", padx=20, pady=6)
btn_start.pack(pady=16)
btn_start.bind("<Enter>", lambda e: e.widget.config(bg="#ff5570"))
btn_start.bind("<Leave>", lambda e: e.widget.config(bg="#e94560"))

# Bottom-right button row: Help, Mute, Game BG (removed Lobby BG)
bottom_frame = tk.Frame(lobby, bg=BG)
bottom_frame.pack(side="bottom", fill="x", padx=10, pady=10)

# Push buttons to the right (transparent spacer)
spacer = tk.Frame(bottom_frame, bg=BG)
spacer.pack(side="left", expand=True)

btn_row = tk.Frame(bottom_frame, bg=BG)
btn_row.pack(side="right")

# Make buttons with transparent effect (inherit parent background)
btn_help = tk.Button(btn_row, text=" ? ", command=show_help,
          bg=BG, fg="#aaaaff", font=("Arial", 11, "bold"),
          relief="flat", bd=0, cursor="hand2", width=3, highlightthickness=0,
          activebackground="#3a4a6e")
btn_help.pack(side="left", padx=2)
btn_help.bind("<Enter>", lambda e: e.widget.config(bg="#3a4a6e"))
btn_help.bind("<Leave>", lambda e: e.widget.config(bg=BG))
ToolTip(btn_help, "How to play")

btn_mute = tk.Button(btn_row, text="🔊", command=lambda: toggle_sound(),
                     bg=BG, fg="#ccccff", font=("Arial", 11, "bold"),
                     relief="flat", bd=0, cursor="hand2", width=3, highlightthickness=0,
                     activebackground="#3a4a6e")
btn_mute.pack(side="left", padx=2)
btn_mute.bind("<Enter>", lambda e: e.widget.config(bg="#3a4a6e"))
btn_mute.bind("<Leave>", lambda e: e.widget.config(bg=BG))
ToolTip(btn_mute, "Mute Music")

btn_bg = tk.Button(btn_row, text="🖼️", command=choose_bg_image,
                   bg=BG, fg="#aaaaff", font=("Arial", 11, "bold"),
                   relief="flat", bd=0, cursor="hand2", width=3, highlightthickness=0,
                   activebackground="#3a4a6e")
btn_bg.pack(side="left", padx=2)
btn_bg.bind("<Enter>", lambda e: e.widget.config(bg="#3a4a6e"))
btn_bg.bind("<Leave>", lambda e: e.widget.config(bg=BG))
ToolTip(btn_bg, "Change Game BG")

# Start lobby music as soon as lobby opens
start_music("lobby_music.wav")


# ─────────────────────────────────────────────────────────────────────────────
#  GAME
# ─────────────────────────────────────────────────────────────────────────────
def run_game(mode, play_mode_str, bg_path, color_pa, color_pb, lobby_window):

    stretched = prepare_background(bg_path)
    start_music("game_music.wav")       # swap lobby music → game music

    window = turtle.Screen()
    window.title("Ping Pong Game")
    window.setup(width=WIN_W, height=WIN_H)
    window.tracer(0)

    # Now that the turtle screen is initialized, destroy the lobby
    try:
        lobby_window.destroy()
    except Exception:
        pass

    if stretched:
        try: window.bgpic(stretched)
        except Exception: window.bgcolor("black")
    else:
        window.bgcolor("black")

    # ── Paddles ───────────────────────────────────────────────────────────────
    DEFAULT_WID = 8

    def make_paddle(x, color):
        p = turtle.Turtle()
        p.speed(0); p.penup()
        p.shape("square")
        p.color(color)
        p.shapesize(stretch_wid=DEFAULT_WID, stretch_len=1)
        p.goto(x, 0)
        p._base_wid = DEFAULT_WID
        p._visible  = True
        return p

    paddle_a = make_paddle(-350, color_pa)
    paddle_b = make_paddle( 350, color_pb)

    # Make bot paddle smaller and harder to hit
    if mode == "pvb":
        paddle_b.shapesize(stretch_wid=5, stretch_len=1)
        paddle_b._base_wid = 5

    # ── Ball ──────────────────────────────────────────────────────────────────
    BASE_SPEED = 0.25
    ball = turtle.Turtle()
    ball.speed(1); ball.shape("circle"); ball.color("white")
    ball.penup(); ball.goto(0, 0)
    ball.dx = BASE_SPEED
    ball.dy = BASE_SPEED

    # ── State ─────────────────────────────────────────────────────────────────
    score_a     = 0
    score_b     = 0
    game_active = True
    last_hitter = None

    # Quick Match: one life each
    lives_a       = 1
    lives_b       = 1
    rally_start   = time.time()
    qm_speed_tier = 0
    
    # Normal mode: rally timer for progressive speed increase every 5 seconds
    normal_rally_start = time.time()
    normal_speed_tier  = 0
    
    # Power-up mode: rally timer for progressive speed increase every 15 seconds
    powerup_rally_start = time.time()
    powerup_speed_tier  = 0

    # ── Score display ─────────────────────────────────────────────────────────
    # ADJUST SCOREBOARD POSITION: Change the Y coordinate below (270) to move up/down
    # Higher = further down, Lower = further up. Range: 220-290 recommended.
    SCOREBOARD_Y = 270  # <-- CHANGE THIS VALUE TO ADJUST VERTICAL POSITION
    
    pen = turtle.Turtle()
    pen.speed(0); pen.penup(); pen.hideturtle()
    pen.goto(0, SCOREBOARD_Y)
    
    # Player names
    player_a_name = "P1"
    player_b_name = "Bot" if mode == "pvb" else "P2"

    def draw_score():
        pen.clear()
        if play_mode_str == "quickmatch":
            ha = "♥" * lives_a + "  " if lives_a > 0 else "☆  "
            hb = "  " + "♥" * lives_b if lives_b > 0 else "  ☆"
            pen.color(color_pa)
            pen.goto(-150, 240)
            pen.write(f"{ha}{player_a_name}", align="center", font=("Press Start 2P", 14, "bold"))
            pen.color(color_pb)
            pen.goto(150, 240)
            pen.write(f"{player_b_name} {hb}", align="center", font=("Press Start 2P", 14, "bold"))
        else:
            pen.color(color_pa)
            pen.goto(-150, 240)
            pen.write(f"{player_a_name}: {score_a}", align="center", font=("Press Start 2P", 20, "bold"))
            pen.color(color_pb)
            pen.goto(150, 240)
            pen.write(f"{player_b_name}: {score_b}", align="center", font=("Press Start 2P", 20, "bold"))
    draw_score()

    # ── HUD ───────────────────────────────────────────────────────────────────
    hud = turtle.Turtle()
    hud.speed(0); hud.penup(); hud.hideturtle()
    hud_msgs = {}

    def hud_show(key, text, duration, x, y, color="yellow"):
        hud_msgs[key] = {"text": text, "expire": time.time() + duration,
                         "x": x, "y": y, "color": color}
        _redraw_hud()

    def hud_clear(key):
        hud_msgs.pop(key, None)
        _redraw_hud()

    def _redraw_hud():
        hud.clear()
        for item in hud_msgs.values():
            hud.color(item["color"])
            hud.goto(item["x"], item["y"])
            hud.write(item["text"], align="center", font=("Press Start 2P", 10, "bold"))

    def expire_hud():
        dead = [k for k, v in hud_msgs.items() if v["expire"] <= time.time()]
        for k in dead: hud_msgs.pop(k)
        if dead: _redraw_hud()

    # ── Paddle movement (continuous key state polling) ───────────────────────
    keys_pressed = set()
    
    def key_down(key):
        keys_pressed.add(key)
    
    def key_up(key):
        keys_pressed.discard(key)
    
    def paddle_move(paddle, dy):
        if not game_active: return
        paddle.sety(max(-220, min(220, paddle.ycor() + dy)))
    
    def process_keys():
        """Poll all pressed keys every frame for simultaneous input."""
        if not game_active: return
        if "w" in keys_pressed:    paddle_move(paddle_a, +1)
        if "s" in keys_pressed:    paddle_move(paddle_a, -1)
        if mode == "pvp":
            if "Up" in keys_pressed:   paddle_move(paddle_b, +1)
            if "Down" in keys_pressed: paddle_move(paddle_b, -1)

    window.listen()
    window.onkeypress(lambda: key_down("w"), "w")
    window.onkeyrelease(lambda: key_up("w"), "w")
    window.onkeypress(lambda: key_down("s"), "s")
    window.onkeyrelease(lambda: key_up("s"), "s")
    if mode == "pvp":
        window.onkeypress(lambda: key_down("Up"), "Up")
        window.onkeyrelease(lambda: key_up("Up"), "Up")
        window.onkeypress(lambda: key_down("Down"), "Down")
        window.onkeyrelease(lambda: key_up("Down"), "Down")

    def ai_move():
        """Bot AI movement. Adjust the +/- threshold values to make bot easier/harder.
        Higher threshold = slower reaction (easier to beat). 
        Current: 80 (very easy difficulty). Try 10 for hard.
        Bot speed is FIXED and does NOT scale with ball speed."""
        if mode == "pvb" and game_active:
            # Fixed bot speed - very slow to make it loseable
            bot_speed = 0.5  # Very slow speed for easy difficulty
            
            if   ball.ycor() > paddle_b.ycor() + 80: paddle_move(paddle_b, +bot_speed)
            elif ball.ycor() < paddle_b.ycor() - 80: paddle_move(paddle_b, -bot_speed)


    # ── End game ──────────────────────────────────────────────────────────────
    def end_game(winner_label):
        nonlocal game_active
        game_active = False
        stop_music()
        play_sfx_blocking("win_music.wav")   # waits for win jingle to finish
        try: 
            messagebox.showinfo("Game Over", f"{winner_label} Won! yippeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeee!")
        except Exception: 
            pass
        try: window.bye()
        except Exception: pass
        if os.path.exists(TEMP_BG):
            try: os.remove(TEMP_BG)
            except Exception: pass
        
        # Return to lobby
        lobby_window.deiconify()
        start_music("lobby_music.wav")

    def check_winner():
        winner_name = player_a_name if score_a >= 10 else player_b_name
        if score_a >= 10: end_game(winner_name); return True
        if score_b >= 10: 
            if mode == "pvb":
                end_game("You weak, Bot wins")
            else:
                end_game(winner_name)
            return True
        return False

    def check_qm_winner():
        if lives_a <= 0: 
            if mode == "pvb":
                end_game("You weak, Bot wins")
            else:
                end_game(player_b_name)
            return True
        if lives_b <= 0: end_game(player_a_name); return True
        return False

    # ── Quick Match: reset ball after each (lost) point ───────────────────────
    def reset_ball_qm(towards):
        nonlocal rally_start, qm_speed_tier
        ball.goto(0, 0)
        ball.color("white")
        sign = 1 if towards == "b" else -1
        ball.dx = sign * BASE_SPEED
        ball.dy = BASE_SPEED
        rally_start   = time.time()
        qm_speed_tier = 0
        hud_clear("speed_tier")

    # ── Quick Match: escalate speed every SPEED_INTERVAL seconds ─────────────
    SPEED_INTERVAL = 10.0    # seconds between speed bumps
    SPEED_FACTOR   = 1.12    # how much to multiply speed each bump
    MAX_TIERS      = 8       # safety cap

    def tick_qm_speed():
        nonlocal qm_speed_tier
        elapsed  = time.time() - rally_start
        tier_now = min(int(elapsed / SPEED_INTERVAL), MAX_TIERS)
        if tier_now > qm_speed_tier:
            qm_speed_tier = tier_now
            factor = SPEED_FACTOR ** qm_speed_tier
            sx = 1 if ball.dx >= 0 else -1
            sy = 1 if ball.dy >= 0 else -1
            ball.dx = sx * BASE_SPEED * factor
            ball.dy = sy * BASE_SPEED * factor
            ball.color("orange")
            window.ontimer(lambda: ball.color("white"), 300)
            hud_show("speed_tier", f"Speed x{factor:.1f}!", 2.5, 0, 210, "orange")

    # ── Normal Mode: escalate speed every 5 seconds ──────────────────────────
    NORMAL_INTERVAL = 5.0
    NORMAL_FACTOR   = 1.15
    MAX_NORMAL_TIERS = 10

    def tick_normal_speed():
        nonlocal normal_speed_tier
        elapsed  = time.time() - normal_rally_start
        tier_now = min(int(elapsed / NORMAL_INTERVAL), MAX_NORMAL_TIERS)
        if tier_now > normal_speed_tier:
            normal_speed_tier = tier_now
            factor = NORMAL_FACTOR ** normal_speed_tier
            sx = 1 if ball.dx >= 0 else -1
            sy = 1 if ball.dy >= 0 else -1
            ball.dx = sx * BASE_SPEED * factor
            ball.dy = sy * BASE_SPEED * factor
            ball.color("cyan")
            window.ontimer(lambda: ball.color("white"), 300)
            hud_show("normal_speed", f"x{factor:.1f} Speed!", 2.5, 0, 210, "cyan")

    # ── Power-up Mode: escalate speed every 15 seconds ───────────────────────
    POWERUP_INTERVAL = 15.0
    POWERUP_FACTOR   = 1.2
    MAX_POWERUP_TIERS = 8

    def tick_powerup_speed():
        nonlocal powerup_speed_tier
        elapsed  = time.time() - powerup_rally_start
        tier_now = min(int(elapsed / POWERUP_INTERVAL), MAX_POWERUP_TIERS)
        if tier_now > powerup_speed_tier:
            powerup_speed_tier = tier_now
            factor = POWERUP_FACTOR ** powerup_speed_tier
            sx = 1 if ball.dx >= 0 else -1
            sy = 1 if ball.dy >= 0 else -1
            ball.dx = sx * BASE_SPEED * factor
            ball.dy = sy * BASE_SPEED * factor
            ball.color("magenta")
            window.ontimer(lambda: ball.color("white"), 300)
            hud_show("powerup_speed", f"x{factor:.1f} Speed!", 2.5, 0, 210, "magenta")

    # ══════════════════════════════════════════════════════════════════════════
    #  POWER-UPS  (only active in "powerups" mode)
    # ══════════════════════════════════════════════════════════════════════════
    POWERUPS = ["long_paddle", "passive_speed", "shrink_opponent",
                "ball_slowdown", "double_points", "invisible_paddle"]

    bubble = turtle.Turtle()
    bubble.speed(0); bubble.shape("circle"); bubble.penup()
    bubble.hideturtle()
    bubble_active     = False
    bubble_spawn_time = 0.0
    bubble_color_idx  = 0
    bubble_colors     = ["#00ffff", "#ff00ff", "#ffff00", "#00ff00", "#ff0080"]  # neon cyan, magenta, yellow, green, pink
    # First bubble appears after 5s; after collection, next one appears after 5s
    next_spawn_delay  = 5.0
    last_bubble_event = time.time()

    original_dx = ball.dx
    original_dy = ball.dy

    effects = {
        "long_a":      {"active": False, "expire": 0},
        "long_b":      {"active": False, "expire": 0},
        "shrink_a":    {"active": False, "expire": 0},
        "shrink_b":    {"active": False, "expire": 0},
        "slowdown":    {"active": False, "expire": 0},
        "invisible_a": {"active": False, "expire": 0},
        "invisible_b": {"active": False, "expire": 0},
        "passive_speed_a": False,
        "passive_speed_b": False,
        "double_a": False,
        "double_b": False,
    }

    def apply_paddle_size(paddle, mult):
        new_wid = max(1, round(paddle._base_wid * mult))
        try: paddle.shapesize(stretch_wid=new_wid, stretch_len=1)
        except Exception: pass

    def restore_paddle_size(paddle):
        try: paddle.shapesize(stretch_wid=paddle._base_wid, stretch_len=1)
        except Exception: pass

    def effective_half(key_long, key_shrink):
        base = 80.0
        if effects[key_long]["active"]:   base *= 1.5
        if effects[key_shrink]["active"]: base *= 0.6
        return base

    def spawn_bubble():
        nonlocal bubble_active, bubble_spawn_time, last_bubble_event, bubble_color_idx
        bubble.goto(random.randint(-280, 280), random.randint(-230, 230))
        bubble.color(bubble_colors[bubble_color_idx])
        bubble.showturtle()
        bubble_active = True
        bubble_spawn_time = last_bubble_event = time.time()
    
    def animate_bubble():
        """Cycle bubble color to create glowing neon effect."""
        nonlocal bubble_color_idx
        if bubble_active:
            bubble_color_idx = (bubble_color_idx + 1) % len(bubble_colors)
            bubble.color(bubble_colors[bubble_color_idx])

    def despawn_bubble():
        """Called only when the ball collects the bubble."""
        nonlocal bubble_active, last_bubble_event, next_spawn_delay
        bubble.hideturtle()
        bubble_active = False
        last_bubble_event = time.time()
        next_spawn_delay = 5.0   # always 5 seconds until next bubble

    def activate_powerup(pu, collector):
        opp   = "b" if collector == "a" else "a"
        now   = time.time()
        pad_c = paddle_a if collector == "a" else paddle_b
        pad_o = paddle_b if collector == "a" else paddle_a
        cx    = -170 if collector == "a" else 170
        ox    =  170 if collector == "a" else -170

        if pu == "long_paddle":
            apply_paddle_size(pad_c, 1.5)
            effects[f"long_{collector}"] = {"active": True, "expire": now + 4}
            hud_show(f"long_{collector}", "Long Paddle 4s", 4, cx, -270, "cyan")
        elif pu == "passive_speed":
            effects[f"passive_speed_{collector}"] = True
            hud_show(f"pspeed_{collector}", "Speed Boost next hit", 6, cx, -250, "orange")
        elif pu == "shrink_opponent":
            apply_paddle_size(pad_o, 0.6)
            effects[f"shrink_{opp}"] = {"active": True, "expire": now + 4}
            hud_show(f"shrink_{opp}", "Shrink! 4s", 4, ox, -270, "magenta")
        elif pu == "ball_slowdown":
            effects["slowdown"] = {"active": True, "expire": now + 5}
            ball.dx /= 2; ball.dy /= 2
            hud_show("slowdown", "Ball Slowdown 5s", 5, 0, 230, "lightblue")
        elif pu == "double_points":
            effects[f"double_{collector}"] = True
            hud_show(f"dbl_{collector}", "2x Points next score", 30, cx, -290, "gold")
        elif pu == "invisible_paddle":
            pad_c.hideturtle(); pad_c._visible = False
            effects[f"invisible_{collector}"] = {"active": True, "expire": now + 3}
            hud_show(f"invisible_{collector}", "Invisible 3s", 3, cx, -250, "#aaffaa")

        play_sfx("powerup.wav")

    def check_effect_expiry():
        now = time.time()
        for side in ("a", "b"):
            pad = paddle_a if side == "a" else paddle_b
            for pfx in ("long", "shrink"):
                key = f"{pfx}_{side}"
                if effects[key]["active"] and now >= effects[key]["expire"]:
                    effects[key]["active"] = False
                    restore_paddle_size(pad)
                    hud_clear(key)
            key = f"invisible_{side}"
            if effects[key]["active"] and now >= effects[key]["expire"]:
                effects[key]["active"] = False
                pad.showturtle(); pad._visible = True
                hud_clear(key)
        if effects["slowdown"]["active"] and now >= effects["slowdown"]["expire"]:
            effects["slowdown"]["active"] = False
            sx = 1 if ball.dx >= 0 else -1
            sy = 1 if ball.dy >= 0 else -1
            ball.dx = sx * abs(original_dx)
            ball.dy = sy * abs(original_dy)
            hud_clear("slowdown")

    def tick_bubble():
        nonlocal bubble_active, last_bubble_event, next_spawn_delay, bubble_spawn_time
        now = time.time()
        if not bubble_active:
            # Spawn when the delay has elapsed; bubble stays until ball touches it
            if now - last_bubble_event >= next_spawn_delay:
                spawn_bubble()

    def check_bubble_collision():
        if not bubble_active: return
        if abs(ball.xcor() - bubble.xcor()) < 18 and abs(ball.ycor() - bubble.ycor()) < 18:
            despawn_bubble()
            activate_powerup(random.choice(POWERUPS), last_hitter or "a")

    def award_point(scorer):
        nonlocal score_a, score_b, normal_rally_start, normal_speed_tier, powerup_rally_start, powerup_speed_tier
        pts = 1
        if play_mode_str == "powerups" and effects.get(f"double_{scorer}"):
            pts = 2
            effects[f"double_{scorer}"] = False
            hud_clear(f"dbl_{scorer}")
        if scorer == "a": score_a += pts
        else:             score_b += pts
        opp = "b" if scorer == "a" else "a"
        if effects.get(f"double_{opp}"):
            effects[f"double_{opp}"] = False
            hud_clear(f"dbl_{opp}")
        draw_score()
        # Reset normal mode rally timer and speed tier
        normal_rally_start = time.time()
        normal_speed_tier = 0
        hud_clear("normal_speed")
        # Reset power-up mode rally timer and speed tier
        powerup_rally_start = time.time()
        powerup_speed_tier = 0
        hud_clear("powerup_speed")

    # ── Main loop ─────────────────────────────────────────────────────────────
    frame_count = 0
    while True:
        window.update()
        if not game_active: break

        process_keys()  # handle simultaneous key presses
        ai_move()
        
        # Animate bubble color every 3 frames (creates glowing effect)
        if play_mode_str == "powerups":
            frame_count += 1
            if frame_count % 3 == 0:
                animate_bubble()

        if play_mode_str == "quickmatch":
            tick_qm_speed()
        elif play_mode_str == "normal":
            tick_normal_speed()
        elif play_mode_str == "powerups":
            tick_powerup_speed()

        ball.setx(ball.xcor() + ball.dx)
        ball.sety(ball.ycor() + ball.dy)

        # Walls
        if ball.ycor() >  290: ball.sety( 290); ball.dy *= -1
        if ball.ycor() < -290: ball.sety(-290); ball.dy *= -1

        # Right border (Player B missed - Player A scores)
        if ball.xcor() > 390:
            play_sfx("miss.wav")
            if play_mode_str == "quickmatch":
                lives_b -= 1
                draw_score()
                if check_qm_winner(): break
                reset_ball_qm("a")
            else:
                ball.goto(0, 0)
                ball.dx = -abs(original_dx); ball.dy = original_dy
                award_point("a")  # Player A scores because B missed
                if check_winner(): break

        # Left border (Player A missed - Player B scores)
        if ball.xcor() < -390:
            play_sfx("miss.wav")
            if play_mode_str == "quickmatch":
                lives_a -= 1
                draw_score()
                if check_qm_winner(): break
                reset_ball_qm("b")
            else:
                ball.goto(0, 0)
                ball.dx = abs(original_dx); ball.dy = original_dy
                award_point("b")  # Player B scores because A missed
                if check_winner(): break

        # Paddle B collision
        half_b = effective_half("long_b", "shrink_b")
        if (345 < ball.xcor() < 355 and
                paddle_b.ycor() - half_b < ball.ycor() < paddle_b.ycor() + half_b):
            ball.setx(340); ball.dx *= -1; last_hitter = "b"
            play_sfx("Boing.wav")
            if play_mode_str == "powerups" and effects["passive_speed_b"]:
                effects["passive_speed_b"] = False
                ball.dx *= 1.6; ball.dy *= 1.6
                ball.color("orange"); hud_clear("pspeed_b")
                window.ontimer(lambda: ball.color("white"), 400)
            else:
                ball.color("white")

        # Paddle A collision
        half_a = effective_half("long_a", "shrink_a")
        if (-360 < ball.xcor() < -340 and
                paddle_a.ycor() - half_a < ball.ycor() < paddle_a.ycor() + half_a):
            ball.setx(-340); ball.dx *= -1; last_hitter = "a"
            play_sfx("Boing.wav")
            if play_mode_str == "powerups" and effects["passive_speed_a"]:
                effects["passive_speed_a"] = False
                ball.dx *= 1.6; ball.dy *= 1.6
                ball.color("orange"); hud_clear("pspeed_a")
                window.ontimer(lambda: ball.color("white"), 400)
            else:
                ball.color("white")

        # Power-up ticks
        if play_mode_str == "powerups":
            check_effect_expiry()
            tick_bubble()
            check_bubble_collision()

        expire_hud()


# ── Launch ────────────────────────────────────────────────────────────────────
lobby.mainloop()
