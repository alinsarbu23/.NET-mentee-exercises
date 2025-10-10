Tasks

[1] Variabile & afișare

Cerință:
Declară variabilele name (string), age (int) și isStudent (bool).
Afișează un mesaj precum: "My name is Alex, I am 22 years old, and student = true."

Explicație: Exersezi declarația de variabile și concatenarea/interpolarea de stringuri.

Input: (nu e necesar – valorile sunt în cod)
Output: My name is Alex, I am 22 years old, and student = True.


[2] Operații aritmetice (inclusiv rest și putere, cu validare)

Cerință:
Citește două numere (double). Afișează: Sum, Diff, Prod, Quot, Mod (% pentru întreagi dacă e posibil), Power (a^b).
Dacă al doilea e 0, la împărțire afișează „NaN”, iar la mod sari peste.

Explicație:
Conversie sigură (TryParse), controale pentru împărțire la zero și operatori suplimentari (Math.Pow). Pentru %, doar dacă ambele sunt întregi.

Input:
a = 8
b = 2


Output:
Sum = 10
Diff = 6
Prod = 16
Quot = 4
Mod = 0
a^b = 64


[3] Par/Impar + multipli & semn

Cerință:
Citește un număr întreg. Spune dacă e par/impar, dacă e multiplu de 3 sau 5 și dacă e negativ/zero/pozitiv.

Explicație: Folosești % 2, % 3, % 5 și o structură if/else if/else pentru semn.

Input:
Number: -15

Output:
Odd, multiple of 3 and 5, negative


[4] Calificativ cu validare + bonus punctual

Cerință:
Citește o notă 0–10. Dacă e în afara intervalului, cere din nou.
Calificative: <5 = Eșuat, 5–6 Suficient, 7–8 Bine, 9 Foarte bine, 10 Excelent.
Dacă nota e cel puțin 9 și ai „bonus = yes”, afișează „+ Bonus”.

Explicație: Loop de validare, categorii cu if/else, branch suplimentar pentru bonus.

Input:
Nota: 11
Nota: 9
Bonus (yes/no): yessw


Output:
Foarte bine + Bonus


[5] Ziua săptămânii + weekend/workday

Cerință:
Citește un număr 1–7 și afișează ziua (EN) + dacă e „Weekend” sau „Workday”.
Dacă numărul nu e valid, afișează „Invalid”.

Explicație: switch sau switch expression, plus o condiție separată pentru weekend (6–7).

Input:
3


Output:
Wednesday – Workday

[6] FizzBuzz extins (N dinamic + conține cifra)

Cerință:
Citește N (1..1000). Afișează 1..N cu reguli:
multiplu 3 → „Fizz”; multiplu 5 → „Buzz”; ambele → „FizzBuzz”;
dacă numărul conține cifra 3, adaugă „” la final (ex: „Fizz”, „7*”).

Explicație: Loop for, reguli de prioritate, conversie i.ToString() pentru „conține 3”.

Input:
N = 16


Output (fragmente):
1
2
Fizz*
4
Buzz
Fizz
7
8
Fizz
Buzz
11
Fizz
FizzBuzz*
13*
14
FizzBuzz


[7] Suma cifrelor + număr de cifre + paritate sumă

Cerință:
Citește un întreg (poate fi negativ). Afișează: SumăCifre, NrCifre (fără semn), SumăCifre par/impar.

Explicație: Normalizezi valoarea cu Math.Abs, împarți repetat la 10, numeri cifrele, verifici paritatea.

Input:
-90210


Output:
Sum = 12, Digits = 5, SumParity = Even


[8] Array – distinct, sortat, mediană

Cerință:
Citește n (3..15), apoi n numere întregi.
Afișează: lista distinctă, sortată crescător, Min, Max, Median.

Explicație: Poți elimina duplicate (set sau Distinct()), sortezi, mediană:

impar: elementul din mijloc;
par: media aritmetică a celor două centrale (double).

Input:
n = 7
[2, 5, 1, 8, 4, 2, 5]


Output:
Distinct sorted: [1, 2, 4, 5, 8]
Min=1, Max=8, Median=4



[9] Ghicește numărul (hot/cold, limită încercări, replay)

Cerință:
Joc 1..100: până la 7 încercări.
După fiecare ghicire: „Too low/Too high”.
Începând cu a doua încercare, spune și „Hotter/Colder” față de distanța anterioară.
La final întreabă „Play again? (Y/N)”.

Explicație: Random, stocare previousGuess, compari |secret - guess|. Contor încercări, validare input.

Input (exemplu):
Guess: 50  -> Too high
Guess: 30  -> Too low, Hotter
Guess: 37  -> Correct in 3 tries!
Play again? (Y/N): N


Output:
Correct in 3 tries!
Goodbye!