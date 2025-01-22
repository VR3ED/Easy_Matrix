from scipy.io import mmread
import numpy as np
import time
from logger import log_results

# rifeerimento ad implementazione di jacobi:
# https://stackoverflow.com/questions/59680833/python-solve-ax-b-using-gradient-descent
def gradient_descent(A,b,x0,max_iters, gamma, tol):
    # initial guess vector x
    next_x = x0
    # print initial f value 
    #print('i = 0 ; f(x)= '+str(f(A,b,next_x)))
    i=1
    # convergence flag
    cvg = False           
    #print('Starting descent')

    while i <= max_iters:
        curr_x = next_x
        next_x = curr_x - gamma * df(A,b,curr_x)

        step = next_x - curr_x
        # convergence test
        if np.linalg.norm(step,2)/(np.linalg.norm(next_x,2)+np.finfo(float).eps) <= tol:
            cvg = True
            break
        # print optionnaly f values while searching for minimum
        #print('i = '+str(i)+' ; f(x)= '+str(f(A,b,next_x)))

        i += 1

    if cvg :
        print('Minimum found in ' + str(i) + ' iterations.')
        print('x_sol =',next_x)
    else :
        print('No convergence for specified parameters.')

    return next_x, i

def f(A,b,x):
    return 0.5*np.dot(np.dot(x,A),x)-np.dot(x,b)

def df(A,b,x):
    return np.dot(A,x)-b

# ---------------------------------------------------------

# file path
file_name = "spa1.mtx"
file_path = f"/home/Cava/Documents/Repos/C#/Easy_Matrix/EasyMatrix/Matrixes/{file_name}"
# r"C:\Users\Cava\Documents\REPOS\C#\Library Metodi del Calcolo Scientifico\EasyMatrix\Matrixes\spa1.mtx"

# matrice A
matrix = mmread(file_name)
A = matrix.toarray()
print(A)

# vettore soluzioni b
b = np.ones(A.shape[0])

# vettore soluzioni x
x = np.zeros(A.shape[0])

# gradient descent parameters
gamma = 0.01          # step size multiplier
tol = 1e-5            # convergence tolerance for stopping criterion
max_iters = 1e10       # maximum number of iterations

# dimension of the problem
n = 10

# Soluzione da libreria scipy che usa gauss-seidel
# link al codice: https://github.com/scipy/scipy/blob/v1.14.0/scipy/sparse/linalg/_dsolve/linsolve.py#L144-L336 
t0 = time.time()
solution, num_iterations = gradient_descent(A, b, x0=x, tol=0.0000000001, max_iters=50000, gamma=gamma)
t1 = time.time()
total = t1-t0

log_results(converge=True,tolerance=0.0000000001, iterations=num_iterations, time_spent=total, matrix_name=file_name, solver_type="GradientSolver")

print("Solution:")
for it in solution:
    print(it)