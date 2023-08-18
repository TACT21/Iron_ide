i = input()
T = [(int(data)) for data in i.split(",")]


for i in range(len(T)-1):
    for j in range(0, len(T)-1-i):
        if T[j] > T[j+1]:
            T[j], T[j+1] = T[j+1], T[j]
    print(T)

input()