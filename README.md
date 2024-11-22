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
