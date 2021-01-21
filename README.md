# auto-complete
Скрипт для инициализации БД
```
IF db_id('DictionaryDB') IS NULL 
    CREATE DATABASE DictionaryDB
GO

CREATE TABLE DictionaryDB.[dbo].[FrequencyDictionary](
	[Word] [nvarchar](450) NOT NULL,
	[Count] [int] NOT NULL,
 CONSTRAINT [PK_FrequencyDictionary] PRIMARY KEY CLUSTERED 
(
	[Word] 
)
) ON [PRIMARY]
GO
```
Для запуска необходимо прописать connectionStrings в конфиги запускаемых файлов.

Примеры запуска ConsoleApp
```
ConsoleApp.exe --init -f input.docx
ConsoleApp.exe --update -f input.txt
ConsoleApp.exe --update -d Directory
ConsoleApp.exe --clear
```
Client 
```
Client.exe --port 8888 --ip 127.0.0.1
```
Server
```
Server.exe --port 8888
Затем ввод команд в формате, как ConsoleApp
```
При старте на чтение, приложение сначала загружает всю таблицу со словарем в память,
это сделано для более быстрого отклика на запросы.

Для разработки:
Как делать миграции:
Добавить команду ef
dotnet tool install --global dotnet-ef

Применить миграции
dotnet ef database update --project Core

Здесь можно скачать корпус английского языка на 15млн слов.
http://www.anc.org/data/oanc/download/
