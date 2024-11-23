# Parallel Name Sorter

A high-performance C# application that demonstrates the power of parallel processing for sorting large lists of names. This implementation shows significant performance improvements over traditional LINQ-based sorting methods.

## Overview

This program reads a list of names from a text file and sorts them by last name, then by full name. It uses a parallel implementation of QuickSort along with other optimization techniques to achieve better performance than the traditional LINQ-based approach.

### Key Features

- Parallel QuickSort implementation
- Efficient name preprocessing using parallel operations
- Adaptive sorting strategy (switches to sequential sort for small partitions)
- Optimized for multi-core processors
- Uses struct-based data structure for better memory efficiency

## Performance Comparison

Original LINQ Implementation vs. Parallel Implementation:

| Test Run | Original (ms) | Optimized (ms) |
|----------|--------------|----------------|
| 1        | 195         | 48             |
| 2        | 244         | 60             |
| 3        | 220         | 51             |
| 4        | 189         | 85             |
| 5        | 185         | 80             |
| Average  | 206.6       | 64.8           |

**Performance Improvement**: The optimized version runs approximately 3.2x faster than the original implementation.

## Code Comparison: Old vs New Implementation

### Original Implementation (LINQ-based)
```csharp
// Old Implementation
class name
{
    public name(string fname, string lname)
    {
        this.firstName = fname; 
        this.lastName = lname;
    }
    public string firstName { get; set; }
    public string lastName { get; set; }
}

// Main sorting logic
List<name> sortedNames = Names.OrderBy(s => s.lastName)
                             .ThenBy(s => s.firstName)
                             .ToList();
```

### New Implementation (Parallel QuickSort)
```csharp
// New Implementation
struct Person
{
    public readonly string FullName;
    public readonly string LastName;

    public Person(string fullName)
    {
        FullName = fullName;
        LastName = ExtractLastName(fullName);
    }
}

// Main sorting using parallel QuickSort
ParallelQuickSort(persons, 0, persons.Length - 1);
```

### Key Differences and Improvements

1. **Data Structure Changes**:
   - Old: Used a `class` with separate firstName and lastName properties
   - New: Uses a `struct` with FullName and extracted LastName for better memory efficiency
   - Benefit: Reduced memory allocation and improved cache locality

2. **Sorting Algorithm**:
   - Old: Used LINQ's `OrderBy` and `ThenBy` operations
   - New: Implements custom parallel QuickSort algorithm
   - Benefit: Better control over parallelization and memory usage

3. **Parallelization**:
   - Old: Relied on LINQ's internal implementation
   - New: Custom parallel implementation with:
     - Configurable threshold for parallel/sequential switching
     - Optimized for multi-core processors
     - Parallel preprocessing of input data

4. **Memory Efficiency**:
   - Old: Created multiple intermediate collections during LINQ operations
   - New: In-place sorting with minimal additional memory allocation
   - Benefit: Reduced memory pressure and garbage collection

5. **Performance Results**:
   - Speed improvement: ~3.2x faster
   - More consistent performance across different data sizes
   - Better scalability with larger datasets

## How to Run

1. Ensure you have .NET Core SDK installed on your system
2. Place your input file named `names.txt` in the same directory as the executable
   - Each line should contain a full name in the format "FirstName LastName"
3. Open a terminal in the project directory
4. Run the following commands:
   ```bash
   dotnet build
   dotnet run
   ```

## Technical Implementation Details

The improved performance is achieved through several optimizations:

1. **Parallel QuickSort**: Implementation of a parallel sorting algorithm that utilizes multiple CPU cores
2. **Efficient Data Structure**: Using a struct-based approach instead of classes to reduce memory overhead
3. **Adaptive Threshold**: Automatically switches to sequential sort for small partitions (< 10,000 items)
4. **Parallel Preprocessing**: Utilizes parallel processing for initial data loading and transformation
5. **Optimized String Comparisons**: Efficient last name extraction and comparison logic

## Input File Format

The program expects a text file named `names.txt` with one name per line:
```
John Smith
Jane Doe
Robert Johnson
```

## System Requirements

- .NET Core SDK 3.1 or higher
- Multi-core processor (for optimal performance)
- Sufficient RAM to hold the entire list of names in memory

1. Person Struct:
   - A `Person` struct was created to hold both the full name and the last name extracted from it. This simplifies access to sorting attributes.
2. Last Name Extraction:
   - The last name is extracted using `LastIndexOf(' ')` to find the last space in the full name string, assuming a "FirstName LastName" format.
3. File Input:
   - Names are read from a file named `names.txt`, and the file is preprocessed to convert each line into a `Person` object using parallel processing to improve performance during the read phase.
4. Parallel QuickSort Implementation:
   - A parallel version of the QuickSort algorithm is implemented. If the size of the array partition is smaller than a defined threshold, it switches to a sequential version for efficiency.
   - `Parallel.Invoke` is used to parallelize the sorting of different partitions of the array, harnessing multiple CPU cores.
5. Comparisons and Swapping:
   - The `ComparePersons` function is used to perform comparisons first by last name and then by full name to handle cases with identical last names.
   - A `Swap` function helps interchange elements in the array.
6. Performance Measurement:
   - A `Stopwatch` is used to time the execution of the sort, and the elapsed time is printed to the console.
