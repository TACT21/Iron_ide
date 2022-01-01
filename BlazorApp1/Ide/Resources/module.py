#Ferrum ide maker:myusan copyright 2021-
# coding: utf-8
import threading
import time

def Input_alt_core(timeout):
    global scope
    for i in range (int(timeout)):
        if(scope.getinput(False)!=""):
            return scope.getinput(True)
        time.sleep(1)

def Input_alt(timeout = 60):
    t = threading.Thread(target=Input_alt_core)
    t.start()
    t.join()
    return t

#END

#IF  YOU WANT TO GET SOMETHING,REPLACE "input()" to "Input_alt()".

#example
#a = input()
#print(a)

#We Must Convert Like This
#a = Input_alt()
#print(a)