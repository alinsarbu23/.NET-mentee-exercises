Tasks

[1] Variabile & afișare

Cerință:
Declară variabilele name (string), age (int) și isStudent (bool).
Afișează un mesaj precum: "My name is Alex, I am 22 years old, and student = true."

Explicație: Exersezi declarația de variabile și concatenarea/interpolarea de stringuri.

Input: (nu e necesar – valorile sunt în cod)
Output: My name is Alex, I am 22 years old, and student = True.


[2] Operații aritmetice

Cerință:
Citește două numere și calculează suma, diferența, produsul și câtul.

Explicație: Folosești operatorii +, -, *, / și conversia de la string la int.

Input:
First number: 8
Second number: 2

Output:
Sum = 10
Diff = 6
Prod = 16
Quot = 4


[3] Par sau impar

Cerință:
Citește un număr și spune dacă e par sau impar.

Explicație:
Un număr este par dacă restul împărțirii la 2 (% 2) este 0.

Input:
Number: 7


Output:
Odd


[4] Calificativ din notă

Cerință:
Citește o notă între 0–10 și afișează calificativul:

sub 5 → „Eșuat”
5–6 → „Suficient”
7–8 → „Bine”
9 → „Foarte bine”
10 → „Excelent”

Explicație: Folosești lanț de if...else if...else și condiții logice.

Input:
Nota (0–10): 9


Output:
Foarte bine


[5] Ziua săptămânii

Cerință:
Citește un număr (1–7) și afișează ziua corespunzătoare.

Explicație: Folosim instrucțiunea switch pentru selecție multiplă.

Input:
Day number (1–7): 3


Output:
Wednesday


[6] FizzBuzz

Cerință:
Afișează numerele 1–100, dar:

multiplu de 3 → „Fizz”
multiplu de 5 → „Buzz”
multiplu de ambele → „FizzBuzz”

Explicație: Folosim o buclă for și condiții multiple cu operatorii % și &&.

Input: (nu se cere)


Output (fragmente):
1
2
Fizz
4
Buzz
Fizz
...


[7] Suma cifrelor
Cerință:
Citește un număr și calculează suma cifrelor sale.

Explicație: Împarți repetat prin 10 (/ 10) și aduni resturile (% 10).

Input:
Number: 123


Output:
Sum of digits = 6


📘 [8] Array – medie, min, max

Cerință:
Citește 5 numere într-un array.
Calculează și afișează minimul, maximul și media.

Explicație: Folosești buclă for pentru citire și foreach pentru procesare.

Input:
a[0]=2
a[1]=5
a[2]=1
a[3]=8
a[4]=4


Output:
Min = 1, Max = 8, Avg = 4


📘 [9] Factorial

Cerință: Citește un număr n și calculează n! (1 × 2 × … × n).

Explicație: Folosești o buclă for și o variabilă acumulatoare result.

Input:
n = 5


Output:
5! = 120


[10] Ghicește numărul

Cerință:
Programul generează un număr aleator între 1 și 100.
Utilizatorul trebuie să-l ghicească.
După fiecare încercare:

„Too low!” dacă e mai mic

„Too high!” dacă e mai mare

„Correct!” + numărul de încercări când ghicește.

Explicație: Se folosește clasa Random, o buclă while (true) și condiții if.

Input:
Guess the number (1..100)!
Your guess: 50
Too high!
Your guess: 20
Too low!
Your guess: 37
Correct! Tries = 3


Output:
(în funcție de rulare, exemplul de mai sus este tipic)