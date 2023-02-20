# checksumcomp
An extremely simple tool for verifying the SHA256 or MD5 hash of a file.

## What is this tool?
This is an extremely simple tool for verifything the algorithmic hash of a downloaded file, without entering the command line.

This is written in C# using the WPF framework. All it does take any given file, generates the hash using the CertUtil command line program which is, by default, installed to the Windows operating system. For that reason, this is only intended to be used with Windows.

Here is an image of the UI:
![ScreenshotUI1](https://user-images.githubusercontent.com/120602813/220208251-039eb639-a408-4c9a-ad46-28b42fe25c2f.png)

## How does this tool work?
It takes the file, which the user provides, and it uses the following command to generate the alorithmic hash of the file:
```
CertUtil -hashfile {path} {algorithm}
```
**{path}** is used to denote the path to the file from which we wish to generate the algorithmic hash.

**{algorithm}** is used to denote the algorithm which will be used to generate the hash. MD5 or SHA256 are the two most commonly used algorithms for verification. For that reason, they are the only included options for this tool.

## Why verify the algorithmic hash of a file?
Verifying the algorithmic hash ensures the integrity of the software being downloaded. Doing so helps prevent the use of malicious software posing as the original trusted software. It also ensures that the software you are using is not corrupted or modified. If the option is present, verifying the hash is generally just a good cyber hygiene. **This is all said with the assumption that the source of the downloaded software is a trusted source.**

This is epecially important when downloading critical software, like Operating Systems. Accidental use of compromised software of this nature can compromise the entire system and potentially result in data theft, loss of critical information, damage to the organization or individual's reputation, and financial losses due to the cost of remediation and potential legal repercussions.
