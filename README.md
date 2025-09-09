# Easy Matrix 

An easy to use library for scientific computing methods 
**EasyMatrix** is a C# library for working with matrices and solving linear systems using iterative methods.
It provides implementations of common solvers such as **Jacobi**, **Gauss-Seidel**, **Gradient Method**, and **Conjugate Gradient**.

[!["Buy Me A Coffee"](https://www.buymeacoffee.com/assets/img/custom_images/orange_img.png)]()


## Matrix files

Matrices must be provided in `.mtx` (Matrix Market) format.
Examples are available in the `Matrixes/` folder.


## Basic usage

Load a matrix from file, create the right-hand side vector **b**,
and solve the system using the Jacobi method:

```csharp
using EasyMatrix;

// Path to the matrix file
string file_path = @"D:\Repos\c#\Easy_Matrix\EasyMatrix\Matrixes\vem1.mtx";

// Create matrix
AccurateMatrix A = new AccurateMatrix(file_path);

// Create RHS vector (all ones)
decimal[] b = Enumerable.Repeat(1.0m, A.columns).ToArray();

// Solver parameters
decimal tol = 0.0001m;
int maxIter = 50000;

// Select solver (Jacobi in this case)
IterativeSolver solver = new JacobiSolver(A, b, tol, maxIter);

// Solve
decimal[] x = solver.Solve();

Console.WriteLine("Solution computed!");
```


## Matrix properties

You can check if a matrix is **symmetric and positive definite**:

```csharp
Console.WriteLine("Is SPD: " + A.IsSymmetricPositiveDefinite());
```


## Available solvers

The library provides the following iterative solvers:

* `JacobiSolver`
* `GaussSeidelSolver`
* `GradientSolver`
* `ConjugateGradientSolver`

### Example: comparing solvers

```csharp
decimal[] tolerances = [0.0001m, 0.000001m];
int maxIter = 50000;

foreach (var tol in tolerances)
{
    IterativeSolver solver = new ConjugateGradientSolver(A, b, tol, maxIter);
    decimal[] result = solver.Solve();
    
    Console.WriteLine($"Solved with {solver.GetType().Name} at tolerance {tol}");
}
```


## Running with multiple matrices

You can test several matrices and solvers in a single run:

```csharp
string[] matrixFiles = ["spa1.mtx", "spa2.mtx", "vem1.mtx", "vem2.mtx"];

foreach (string matrixName in matrixFiles)
{
    AccurateMatrix A = new AccurateMatrix(@"Path\to\Matrixes\" + matrixName);
    decimal[] b = Enumerable.Repeat(1.0m, A.columns).ToArray();

    foreach (var solver in new IterativeSolver[]
    {
        new JacobiSolver(A, b, 0.0001m, maxIter),
        new GaussSeidelSolver(A, b, 0.0001m, maxIter),
        new GradientSolver(A, b, 0.0001m, maxIter),
        new ConjugateGradientSolver(A, b, 0.0001m, maxIter)
    })
    {
        decimal[] result = solver.Solve();
        Console.WriteLine($"[{matrixName}] {solver.GetType().Name} completed!");
    }
}
```


## Requirements

* .NET 6 or higher
* Matrix files in `.mtx` format


