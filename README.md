# Custom Hash Tool

This project provides a command-line tool for computing hashes using built-in and custom hash algorithms, as well as an automated testing suite for evaluating hash properties.

## Features

- Supports multiple hash algorithms:
  - `hashv01` - Custom hash implementation version 01.
  - `sha256` - Standard SHA-256.
  - `md5` - Standard MD5.
- Can hash either:
  - A text string provided via command line.
  - The contents of a file located in the `Data` folder.
- Interactive input mode if no file or string is provided.
- Generates detailed output:
  - Raw bytes of input and hash.
  - Hexadecimal representation.
  - UTF-8 string representation (if printable).
- Automated test suite (`--test`) for:
  - Fixed-length hashing.
  - Determinism.
  - Collision resistance.
  - Avalanche effect.
  - Speed analysis.


## Usage

### Command-Line Arguments
```
Usage: dotnet run <hash_algorithm> [input] [--test]

Arguments:
<hash_algorithm> Name of the hash algorithm to use.
[input] Optional. Text string or path to a file in the Data folder to hash.
[--test] Optional. Run the automated test suite.
```

### Available Hash Algorithms

- `hashv01` - Custom Hash version 01.
- `sha256` - Standard SHA-256.
- `md5` - Standard MD5.

### Examples

- Hash a string with SHA-256:

```bash
dotnet run sha256 "Hello World"
```

- Hash a file from the `Data` folder:

```bash
dotnet run hashv01 input.txt
```

- Run the automated test suite using MD5:

```bash
dotnet run md5 --test
```

---
# Hash Function Test Results

This document aggregates the results of automated hash function tests performed on different algorithms.

## Summary Table

| Algorithm     | Fixed Length | Determinism | Collision Accuracy | Avalanche (Bits) Min | Max   | Avg   | Avalanche (Hex) Min | Max   | Avg   |
|---------------|--------------|-------------|--------------------|----------------------|-------|-------|---------------------|-------|-------|
| **SHA-256**   | 100%         | 100%        | 100%               | 37.11%               | 63.67%| 50.01%| 78.13%              | 100%  | 93.75%|
| **SHA-1**     | 100%         | 100%        | 100%               | 33.13%               | 66.88%| 50.01%| 70.00%              | 100%  | 93.76%|
| **MD5**       | 100%         | 100%        | 100%               | 30.47%               | 74.22%| 50.01%| 71.88%              | 100%  | 93.74%|
| **Hash v0.1** | 100%         | 100%        | 100% *(misleading)*| 0%                   | 54.69%| 1.76% | 0%                  | 98.44%| 3.41% |
| **Hash v0.2** | 100%         | 100%        | 100%               | 0.39%                | 59.38%| 10.50%| 1.56%               | 100%  | 21.49%|
| **Hash v0.3** | 100%         | 100%        | 100%               | 0.39%                | 61.72%| 12.20%| 1.56%               | 100%  | 23.10%|
| **Hash v1.0** | 100%         | 100%        | 100%               | 0.39%                | 65.63%| 32.67%| 1.56%               | 100%  | 61.3% |
| **Hash v1.1** | 100%         | 100%        | 100%               | 36.33%               | 65.62%| 49.99%| 76.56%              | 100%  | 93.74%|

---

**Note on Collisions for Hashv0.1:**  
The collision testing suggested by the lecturer is not exhaustive as it only checks for collisions between random strings.  
My current implementation produces **no collisions for text input <= 256 bits** (as far as I can tell), but **any input longer than 256 bits is truncated**, leading to inevitable collisions in practice.
This is apparent in the avalanche testing (min values at 0% and overall average very low).

## Speed results

<img width="1068" height="629" alt="image" src="https://github.com/user-attachments/assets/c7d2d0cf-8482-43ee-9e2f-b79040c3c3b3" />

### SHA256
<img width="376" height="226" alt="image" src="https://github.com/user-attachments/assets/0f60b82d-24a6-43ab-8454-1ea1c0b93703" />
<img width="376" height="226" alt="image" src="https://github.com/user-attachments/assets/8cd2c0bd-00ce-44a9-9252-c9c26e740d34" />

### MD5
<img width="376" height="226" alt="image" src="https://github.com/user-attachments/assets/b420b1d4-69ed-413a-8a66-b0e12cdcdade" />
<img width="376" height="226" alt="image" src="https://github.com/user-attachments/assets/48b26df6-3e78-443a-8a2c-1194dd532e21" />

### Hash v0.1
<img width="376" height="226" alt="image" src="https://github.com/user-attachments/assets/771dad6b-3774-4c73-87aa-837f35ccd137" />
<img width="376" height="226" alt="image" src="https://github.com/user-attachments/assets/e595521e-bae0-407c-847b-8beada463b18" />
