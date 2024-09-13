import os
import platform
import time

def log_results(converge, tolerance, iterations, time_spent, matrix_name, solver_type):
    file_path = "librerie_esterne.csv"
    file_exists = os.path.exists(file_path)

    with open(file_path, "a") as writer:
        # Se il file non esiste, scrive l'intestazione
        if not file_exists:
            writer.write("Matrix,Convergence,SolverType,PrecisionRequired,Iterations,TimeSpent(ms),OS\n")
        
        os_type = "Linux" if platform.system() == "Linux" else "Windows"
        
        # Scrive i dati
        writer.write(f"{matrix_name},{converge},{solver_type},{tolerance},{iterations},{time_spent * 1000},{os_type}\n")
