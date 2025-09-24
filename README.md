# Custom Hash Tool

This project provides a command-line tool for computing hashes using built-in and custom hash algorithms, as well as an automated testing suite for evaluating hash properties.

## Features

- Supports multiple hash algorithms:
  - `hashv01` – Custom hash implementation version 01.
  - `sha256` – Standard SHA-256.
  - `md5` – Standard MD5.
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
--test Optional. Run the automated test suite.
```

### Available Hash Algorithms

- `hashv01` – Custom Hash version 01.
- `sha256` – Standard SHA-256.
- `md5` – Standard MD5.

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
|---------------|--------------|-------------|--------------------|-----------------------|-------|-------|----------------------|-------|-------|
| **SHA-256**   | 100%         | 100%        | 100%               | 37.11%               | 63.67%| 50.01%| 78.13%              | 100%  | 93.75%|
| **MD5**       | 100%         | 100%        | 100%               | 30.47%               | 74.22%| 50.01%| 71.88%              | 100%  | 93.74%|
| **Hash v0.1** | 100%         | 100%        | 100% *(misleading)*| 0%                   | 54.69%| 1.76% | 0%                  | 98.44%| 3.41% |
| **Hash v0.2** | *TBD*        | *TBD*       | *TBD*              | *TBD*                | *TBD* | *TBD* | *TBD*               | *TBD* | *TBD* |
---

**Note on Collisions for Hashv0.1:**  
The collision testing suggested by the lecturer is not exhaustive as it only checks for collisions between random strings.  
My current implementation produces **no collisions for text input ≤ 256 bits** (as far as I can tell), but **any input longer than 256 bits is truncated**, leading to inevitable collisions in practice.
This is apparent in the avalanche testing (min values at 0% and overall average very low).

## Speed results

### SHA256
<img width="376" height="226" alt="image" src="https://github.com/user-attachments/assets/0f60b82d-24a6-43ab-8454-1ea1c0b93703" />
<img width="376" height="226" alt="image" src="https://github.com/user-attachments/assets/8cd2c0bd-00ce-44a9-9252-c9c26e740d34" />

### MD5
<img width="376" height="226" alt="image" src="https://github.com/user-attachments/assets/b420b1d4-69ed-413a-8a66-b0e12cdcdade" />
<img width="376" height="226" alt="image" src="https://github.com/user-attachments/assets/48b26df6-3e78-443a-8a2c-1194dd532e21" />

### Hash v0.1
<img width="376" height="226" alt="image" src="https://github.com/user-attachments/assets/771dad6b-3774-4c73-87aa-837f35ccd137" />
<img width="376" height="226" alt="image" src="https://github.com/user-attachments/assets/e595521e-bae0-407c-847b-8beada463b18" />
