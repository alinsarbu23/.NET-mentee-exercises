Cerinta: Aplicatie de tip consola ce simuleaza un sistem bancar.
Obiectiv: Stocarea unei liste de conturi in memorie si afisarea unui meniu in consola cu diferite operatiuni pe acestea:
1. Listare conturi
2. Adaugare cont
3. Deposit (depunere suma in cont)
4. Withdraw (retragere suma din cont)
5. View statement (istoric tranzactii)
6. Run month-end (aplicare reguli de sfarsit de luna (dobanzi) pentru toate conturile)
7. Exit (iesire din aplicatie

Tipuri de conturi:
	1. CheckingAccount	- Permite overdraft (sold negativ) până la o limită, ex: -200.
						- Nu se aplică dobândă la sfârșit de lună.

	2. SavingsAccount	- Nu permite overdraft; la sfârșit de lună se adaugă dobândă de 1% dacă soldul > 0.
						- Nu poti avea sold negativ.
						
	3. LoanAccount		- Soldul este negativ (datorie). „Deposit” = rambursare (soldul se apropie de 0), „Withdraw” = împrumut suplimentar (soldul devine mai negativ). Se aplică dobândă la sfârșit de lună.
						- La sfârșit de lună se aplică o dobândă de 2% la soldul negativ.	

Validari:
a. Conturi cu sold negativ de pana la -200
b. Economii: daca un cont e strict pozitiv, castiga 1% dobanda la optiunea 6
c. Dobanda lunara se acumuelaza la datoria lunara
d. Toate intrarile valdiate (id, sume, campuri etc)
	
