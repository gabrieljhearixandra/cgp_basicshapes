from tkinter import *
window = Tk()
window.geometry("400x300")
window.title("Jhea Assignment 1")
window.config(bg="#ADBDE9")

Canvas = Canvas(window, width=400, height=300)
Canvas.config(bg="#ADDAF0")
Canvas.pack()



#right arm sa view ko
Canvas.create_polygon(253, 152, 280, 110, 290, 140, 253, 193, outline="black", fill="#03ADF0") # right hand
#right palm na color white, yung bilog sa dulo ng kamay
Canvas.create_oval(275, 95, 310, 140, fill="#FFFFFF") # right palm
#left arm
Canvas.create_polygon(120, 170, 153, 140, 170, 180, 120, 200, fill="#03ADF0", outline="black") # left hand 
#left palm
Canvas.create_oval(100, 170, 135, 210, fill="#FFFFFF") # left palm

#body nya
Canvas.create_polygon(150, 125, 250, 125, 265, 250, 130, 250, fill="#03ADF0", outline="black")


# head nya
Canvas.create_oval(125, 25, 275, 175, fill="#03ADF0") # main head
Canvas.create_oval(135, 55, 265, 170, fill="#DAE5EA") #yung face
Canvas.create_oval(200, 33, 230, 80, fill="#DAE5EA") #right eye
Canvas.create_oval(169, 33, 199, 80, fill="#DAE5EA") #left eye
Canvas.create_oval(185, 75, 215, 100, fill="#C21616") #rudolf
Canvas.create_oval(197, 77, 207, 85, fill="#FFFFFF") #yung white sa ilong

#eye pupils
Canvas.create_oval(205, 50, 215, 70, fill="#000000") # right pupil
Canvas.create_oval(185, 50, 195, 70, fill="#000000") # left pupil

#white eye pupils
Canvas.create_oval(210, 55, 215, 65, fill="#FFFFFF") # right white pupil
Canvas.create_oval(190, 55, 195, 65, fill="#FFFFFF") # left white pupil

#whiskers
Canvas.create_line(199, 101, 199, 145, fill="black", width=2) # yung gitnang line sa mukha
Canvas.create_line(180, 130, 140, 140, fill="black", width=2) # left baba whisker
Canvas.create_line(180, 120, 140, 120, fill="black", width=2) # left middle whisker
Canvas.create_line(180, 110, 140, 100, fill="black", width=2) # left upper whisker
Canvas.create_line(220, 130, 260, 140, fill="black",  width=2) # right baba whisker
Canvas.create_line(220, 120, 260, 120, fill="black",  width=2) # right middle whisker
Canvas.create_line(220, 110, 260, 100, fill="black",  width=2) # right upper whisker

#yung paa naman
Canvas.create_oval(125, 240, 185, 280, fill="#FFFFFF") # left foot
Canvas.create_oval(210, 240, 270, 280, fill="#FFFFFF") # right foot

#square tapos yung name nya
Canvas.create_rectangle(20, 40, 70, 260, outline="black",width=2, fill="#FFFFFF") 
text1 = Canvas.create_text(45, 150, text="D\nO\nR\nA\nE\nM\nO\nN", font=("Arial", 16, "bold"), fill="black", ) 



#name ko
Canvas.create_text(100, 290, text="Jhea Rixandra P. Gabriel", font=("Arial", 12, "bold"), fill="black")

#section
Canvas.create_text(350, 290, text="BSIT3-PM", font=("Arial", 12, "bold"), fill="black")

window.mainloop()