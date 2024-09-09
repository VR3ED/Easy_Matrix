import numpy as np
from scipy.io import mmread
from scipy.sparse.linalg import spsolve
from scipy.sparse import csc_matrix
from scipy.sparse.linalg import cg
import time
from logger import log_results


# file path
file_name = "vem2.mtx"
file_path = f"/home/Cava/Documents/Repos/C#/Easy_Matrix/EasyMatrix/Matrixes/{file_name}"
# r"C:\Users\Cava\Documents\REPOS\C#\Library Metodi del Calcolo Scientifico\EasyMatrix\Matrixes\spa1.mtx"

# matrice A
matrix = mmread(file_path)
A = csc_matrix(matrix.toarray())

# vettore soluzioni b
b = np.ones(A.shape[0])

# Variable to track number of iterations
num_iterations = 0
# Define the callback function
def callback(xk):
    global num_iterations
    num_iterations += 1

# solve
# rifermenti:
# codice: https://github.com/scipy/scipy/tree/main/scipy/sparse/linalg
# documentazione: https://docs.scipy.org/doc/scipy/reference/generated/scipy.sparse.linalg.cg.html
t0 = time.time()
x, exit_code = cg(A, b, atol=1e-10, maxiter=50000, callback=callback)
t1 = time.time()
total = t1-t0

log_results(converge=exit_code==0,tolerance=0.0000000001, iterations=num_iterations, time_spent=total, matrix_name=file_name, solver_type="ConjugateGradientSolver")

# print
print(exit_code)    # 0 indicates successful convergence
print("Solution:")
for it in x:
    print(it)
