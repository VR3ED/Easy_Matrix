from scipy.io import mmread
from scipy.sparse.linalg import spsolve
from scipy.sparse import csr_matrix
import numpy as np
import time
from logger import log_results

# rifeerimento ad implementazione di jacobi:
# https://stackoverflow.com/questions/5622656/python-library-for-gauss-seidel-iterative-solver
def gauss_seidel(A, b, tolerance, max_iterations, x):
    #x is the initial condition
    iter1 = 0
    #Iterate
    for k in range(max_iterations):
        iter1 = iter1 + 1
        #print ("The solution vector in iteration", iter1, "is:", x)    
        x_old  = x.copy()
        
        #Loop over rows
        for i in range(A.shape[0]):
            x[i] = (b[i] - np.dot(A[i,:i], x[:i]) - np.dot(A[i,(i+1):], x_old[(i+1):])) / A[i ,i]
            
        #Stop condition 
        #LnormInf corresponds to the absolute value of the greatest element of the vector.\
        
        LnormInf = max(abs((x - x_old)))/max(abs(x_old))   
        #print ("The L infinity norm in iteration", iter1,"is:", LnormInf)
        if  LnormInf < tolerance:
            break
         
    return x, iter1

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

# vettore soluzioni x
x = np.zeros(A.shape[0])

# Soluzione da libreria scipy che usa gauss-seidel
# link al codice: https://github.com/scipy/scipy/blob/v1.14.0/scipy/sparse/linalg/_dsolve/linsolve.py#L144-L336 
t0 = time.time()
solution, num_iterations = gauss_seidel(A, b, tolerance=0.0000000001, max_iterations=50000, x=x)
t1 = time.time()
total = t1-t0

log_results(converge=True,tolerance=0.0000000001, iterations=num_iterations, time_spent=total, matrix_name=file_name, solver_type="GaussSeidelSolver")

print("Solution:")
for it in solution:
    print(it)
