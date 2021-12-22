import threading
import time



def loop():
    x = 2
    for i in range (10):
        x = x ** 2
        print(x)
        time.sleep(1)

t = threading.Thread(target=loop)
t.start()
for i in range (8):
    print("waiting")
    time.sleep(1)
t.join()
print("end")