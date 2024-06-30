from scipy.io import mmread
from scipy.sparse.linalg import spsolve
from scipy.sparse import csr_matrix
import numpy as np

# matrice A
matrix = mmread(r"C:\Users\Cava\Documents\REPOS\C#\Library Metodi del Calcolo Scientifico\EasyMatrix\Matrixes\spa1.mtx")
A = matrix.toarray()

# vettore soluzioni b
b = np.ones(A.shape[0])

# Soluzione da libreria scipy che usa gauss-seidel
# link al codice: https://github.com/scipy/scipy/blob/v1.14.0/scipy/sparse/linalg/_dsolve/linsolve.py#L144-L336 
solution = spsolve(A, b)

print("Solution:")
for it in solution:
    print(it)
