from scipy.io import mmread
import numpy as np
from numpy import array, zeros, diag, diagflat, dot
import pyamg

# rifeerimento ad implementazione di jacobi:
# https://gist.github.com/angellicacardozo/3a0891adfa38e2c4187612e57bf271d1 
def Jacobi(A, b, N, x):
                                                                                                                                                                   
    # (1) Create a vector using the diagonal elemts of A
    D = diag(A)
    # (2) Subtract D vector from A into the new vector R
    R = A - diagflat(D)

    # (3) We must Iterate for N times                                                                                                                                                                          
    for i in range(N):
        x = (b - dot(R,x)) / D
    return x



# matrice A
matrix = mmread(r"C:\Users\Cava\Documents\REPOS\C#\Library Metodi del Calcolo Scientifico\EasyMatrix\Matrixes\spa1.mtx")
A = matrix.toarray()

# vettore soluzioni b
b = np.ones(A.shape[0])

# vettore x
x0 = np.zeros_like(b)

sol = Jacobi(A=A, b=b, x=x0, N=50000)

print('Solution:')
for it in sol:
    print(it)

# Create a PyAMG solver using Jacobi method
#ml = pyamg.ruge_stuben_solver(A)
#jacobi_solver = pyamg.relaxation.relaxation.jacobi(A=A, b=b,x=x0)
#print('Solution:', sol)