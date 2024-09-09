from scipy.io import mmread
import numpy as np
from numpy import array, zeros, diag, diagflat, dot
import pyamg
import time
from logger import log_results


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

# ---------------------------------------------------------

# file path
file_name = "vem2.mtx"
file_path = f"/home/Cava/Documents/Repos/C#/Easy_Matrix/EasyMatrix/Matrixes/{file_name}"
# r"C:\Users\Cava\Documents\REPOS\C#\Library Metodi del Calcolo Scientifico\EasyMatrix\Matrixes\spa1.mtx"

# matrice A
matrix = mmread(file_path)
A = matrix.toarray()

# vettore soluzioni b
b = np.ones(A.shape[0])

# vettore x
x0 = np.zeros_like(b)

# solve
t0 = time.time()
sol = Jacobi(A=A, b=b, x=x0, N=50000)
t1 = time.time()
total = t1-t0

log_results(converge=True,tolerance=0.0001, iterations=50000, time_spent=total, matrix_name=file_name, solver_type="JacobiSolver")

print('Solution:')
for it in sol:
    print(it)

# Create a PyAMG solver using Jacobi method
#ml = pyamg.ruge_stuben_solver(A)
#jacobi_solver = pyamg.relaxation.relaxation.jacobi(A=A, b=b,x=x0)
#print('Solution:', sol)