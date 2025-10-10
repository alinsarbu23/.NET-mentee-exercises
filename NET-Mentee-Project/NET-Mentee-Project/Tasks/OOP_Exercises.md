
C# Practice – 1 Easy, 2 Medium, 2 Hard (Ch. 1–7)
=================================================

[E1] Easy – Contact Card (Class, Properties, Validation, ToString)
------------------------------------------------------------------
Cerință:
Creează o clasă Contact cu proprietăți:
- Name (string, obligatoriu, min 2 caractere)
- Age (int, 0..130)
- Email (string, opțional; dacă e prezent, trebuie să conțină '@')
- IsStudent (bool)
Expune ToString() care întoarce: "Name (Age) – student=True/False – email: <email or n/a>".
Aplică validări în setteri (ArgumentException pe date invalide).
Programul citește un singur contact dintr-o linie și îl afișează.

Input (exemplu):
Alice 22 alice@mail.com true

Output (exemplu):
Alice (22) – student=True – email: alice@mail.com


[M1] Medium – Playlist Manager (Classes, Encapsulation, Aggregation)
--------------------------------------------------------------------
Cerință:
Creează clasele Song(title, artist, durationSeconds>=1) și Playlist(name).
Playlist conține o listă privată de Song și expune metode:
- Add(Song), Remove(title), FindByArtist(artist) -> IEnumerable<Song>
- TotalDuration() -> TimeSpan
- Longest() -> Song
Programul citește comenzi până la END:
ADD title;artist;durationSec
REMOVE title
FINDARTIST artist
TOTAL
LONGEST
LIST
END
Afișează rezultatele potrivit comenzilor.

Input (exemplu):
ADD Viva La Vida;Coldplay;242
ADD Yellow;Coldplay;270
ADD Numb;LinkinPark;185
FINDARTIST Coldplay
LONGEST
TOTAL
LIST
END

Output (exemplu):
Viva La Vida – Coldplay (242s)
Yellow – Coldplay (270s)
Yellow – Coldplay (270s)
Total: 697s (00:11:37)
1) Viva La Vida – Coldplay (242s)
2) Yellow – Coldplay (270s)
3) Numb – LinkinPark (185s)


[M2] Medium – Library (Readonly ID, Properties, Simple Search)
-------------------------------------------------------------
Cerință:
Creează clasa Book cu:
- Id (Guid) doar get (generat în constructor)
- Title (string, non-empty), Author (string, non-empty), Year (int, 1450..2100), Available (bool)
Metode: Borrow() și Return() care schimbă Available; dacă împrumutul e imposibil, aruncă InvalidOperationException.
Programul citește comenzi:
ADD title;author;year
BORROW title
RETURN title
FIND titleSubstring
LIST
END

Input (exemplu):
ADD Clean Code;Robert C. Martin;2008
ADD CLR via C#;Jeffrey Richter;2012
BORROW Clean Code
FIND C#
LIST
END

Output (exemplu):
OK
OK
OK
CLR via C# – Jeffrey Richter (2012) – Available=True
1) Clean Code – Robert C. Martin (2008) – Available=False
2) CLR via C# – Jeffrey Richter (2012) – Available=True


[H1] Hard – Banking v2 (Abstract Class, Overrides, Polymorphism)
----------------------------------------------------------------
Cerință:
Definește clasa abstractă Account cu:
- Iban (string, valid simplu: începe cu "RO", non-empty), Owner (string, non-empty)
- Balance (decimal, doar get)
- Deposit(decimal amount>0) (virtual)
- Withdraw(decimal amount>0) (abstract)
Derivează:
- CheckingAccount: comision fix la withdraw (ex: 2.50)
- SavingsAccount: interzice să scadă sub 100.00; are metodă ApplyInterest(decimal ratePct>0) care mărește Balance.
Programul gestionează 2 conturi (C și S), execută comenzi și afișează soldurile.
Comenzi:
DEPOSIT C 500
DEPOSIT S 800
WITHDRAW C 120
WITHDRAW S 50
INTEREST S 3.5
BALANCE ALL
END

Output (exemplu):
OK
OK
OK
ERROR (min balance 100.00)
OK
C: <balance_c>
S: <balance_s>


[H2] Hard – Parking Lot (Inheritance, Polymorphism, Rules)
----------------------------------------------------------
Cerință:
Creează o ierarhie de vehicule: Vehicle (abstract) cu properties: Plate, EnterTime (DateTime), ExitTime? (DateTime?).
Derivează: Car, Motorcycle, Truck.
Parcarea are reguli de tarifare per oră (rounded-up):
- Car: 5/unitate oră
- Motorcycle: 3/unitate oră
- Truck: 10/unitate oră + suprataxă fixă 15 dacă durata depășește 4h
Creează clasa ParkingLot cu metode:
- Enter(Vehicle v) – salvează intrarea; nu permite dubluri de plăcuță active
- Exit(plate, exitTime) – setează ExitTime și calculează tariful
- Report() – listează vehiculele în parcare și încasările totale
Comenzi:
ENTER type plate yyyy-MM-ddTHH:mm
EXIT plate yyyy-MM-ddTHH:mm
REPORT
END

Input (exemplu):
ENTER Car B-01-ABC 2025-10-10T08:00
ENTER Truck CJ-55-XYZ 2025-10-10T09:15
EXIT B-01-ABC 2025-10-10T11:10
EXIT CJ-55-XYZ 2025-10-10T15:45
REPORT
END

Output (exemplu):
OK
OK
B-01-ABC -> 5*4h=20
CJ-55-XYZ -> 10*7h + 15 = 85
Total: 105
(Empty lot)
