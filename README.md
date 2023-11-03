# EmployeesManagementApp

Instrukcja instalacji:
1. Pobieramy projekt klikając CODE -> Download Zip
2. Wypakowujemy
3. Uruchamiamy za pomocą Visual Studio plik EmployeesManagementApp.sln z folderu EmployeesManagementApp-master
4. Przechodzimy do zakładki na dole programu Package Manager Console, gdzie wyświetli się komunikat z przyciskiek restore i klikamy go
5. Jeśli mamy bazę danych SQL Server na lokalnym komputerze, to wpisujemy nasepującą komendę w Package Manager Console:
update-database -ConnectionProviderName System.Data.SqlClient -ConnectionString "Server = (local)\SQLEXPRESS; Database = EmployeesManagementApp; User Id = (twoja nazwa użytkownika); Password = (twoje hasło);"
(local) to dmyślny adres serwera na komputerze lokalnym, a SQLEXPRESS to domyślna nazwa servera.
6. Następnie uruchamiamy aplikację
7. Zostaniemy poproszeni o wpisanie danych do połączenia się z bazą danych. Należy wpisać te same dane, które przakazaliśmy w pkt. 5 do ConnectionString-a
8. Następnie aplikacja się zrestartuje i utworzy 2 konta użytkoników:
- nazwa użytkownika: Mark, hasło: Mark,
- nazwa użytkownika: John, hasło: John
Należy się zalogować na jedno z nich
9. Mozemy teraz korzystać z naszej aplikacji!
