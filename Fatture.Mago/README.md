# Programma di Esempio - TransferMNET

Questo è un **programma di esempio completo e documentato** che dimostra come utilizzare la libreria `Maxidata.TransferMNET.dll` per sincronizzare fatture con Mago.NET.

## Obiettivo

Fornire un **esempio pratico e pronto all'uso** che mostra la logica completa del metodo `SyncInvoice` utilizzando **esclusivamente** la libreria TransferMNET, con gestione avanzata della connessione, clienti, fatture e configurazione centralizzata.

## Struttura del Progetto

```
ProgrammaDiEsempio/
├── Program.vb                     ← Programma principale con logica completa
├── MagoConnectionHelper.vb        ← Helper per gestione connessioni
├── Enum.vb                        ← Enumerazioni Mago.NET
├── My Project/AssemblyInfo.vb     ← Info assembly
├── ProgrammaDiEsempio.vbproj     ← File progetto
├── ProgrammaDiEsempio.sln        ← Solution Visual Studio
└── README.md                      ← Questa documentazione
```

## Caratteristiche Principali

### Documentazione Completa
- **Commenti XML** per tutte le classi e metodi
- **Documentazione inline** dettagliata per ogni fase del processo
- **Struttura organizzata** con sezioni logiche ben definite

### Configurazione Centralizzata
Il programma include una sezione **"PARAMETRI DI CONFIGURAZIONE"** chiaramente marcata con tutti i parametri personalizzabili:

```vb
' >>> CONFIGURAZIONE CONNESSIONE MAGO.NET <<<
Public Const MAGO_SERVER_IP As String = "10.21.12.27"
Public Const MAGO_INSTANCE As String = "MAGO4"
Public Const MAGO_COMPANY As String = "DEMO_Mago4"
Public Const MAGO_USERNAME As String = "Maxidata_1"
Public Const MAGO_PASSWORD As String = ""
Public Const MAGO_LICENSE As String = "0110C137"

' >>> CONFIGURAZIONE PARAMETRI AZIENDALI <<<
Public Const CODICE_PAGAMENTO As String = "CONT"
Public Const REGISTRO_IVA As String = "VEN"
Public Const MODELLO_CONTABILE As String = "FE"
Public Const CODICE_IVA_STANDARD As String = "22"
```

### Gestione Connessioni Avanzata
- **Classe MagoConnectionHelper** per gestione centralizzata delle connessioni
- **Gestione errori completa** con messaggi descrittivi per tutti i codici di errore
- **Validazione parametri** automatica
- **Configurazione automatica** directory di log

## Dipendenze

- **Unica dipendenza**: `Maxidata.TransferMNET.dll` (tramite project reference)
- .NET Framework 4.0 Client Profile
- Visual Basic.NET

## Funzionalità Implementate

### 1. Gestione Connessione
- Apertura e chiusura sicura delle connessioni Mago.NET
- Configurazione automatica directory di log
- Gestione centralizzata degli errori con messaggi descrittivi

### 2. Gestione Clienti
- Verifica esistenza clienti nel database
- Creazione automatica clienti mancanti
- Configurazione completa dati anagrafici

### 3. Creazione Fatture
- Setup container `mdInvoiceList`
- Configurazione completa campi fattura
- Gestione importi, IVA e dati contabili

### 4. Righe di Dettaglio
- Aggiunta righe con `AggiungiDetailRow()`
- Configurazione servizi e prestazioni
- Calcolo automatico importi

### 5. Sincronizzazione
- Scrittura definitiva dati a Mago.NET
- Validazione risultati operazioni
- Logging dettagliato di tutte le fasi

## Utilizzo

### 1. Configurazione
Modificare i parametri nella sezione **"PARAMETRI DI CONFIGURAZIONE"** del file `Program.vb`:
- Parametri di connessione Mago.NET
- Codici aziendali (pagamento, IVA, registro, etc.)
- Configurazioni documenti

### 2. Compilazione
```bash
# Da Visual Studio
Build → Build Solution

# Da riga di comando
msbuild ProgrammaDiEsempio.sln
```

### 3. Esecuzione
```bash
ProgrammaDiEsempio.exe
```

### 4. Modalità Produzione
Per utilizzare in produzione, decommentare le righe:
```vb
connectionHelper.OpenConnection()
customersList.Scrivi()
invoiceList.Scrivi()
```

## Output di Esempio

```
=== PROGRAMMA DI ESEMPIO - TransferMNET ===
Sincronizzazione fatture con Mago.NET
==================================================

FASE 1: Configurazione connessione a Mago.NET
--------------------------------------------------
ATTENZIONE: Configurare i parametri di connessione reali prima dell'uso in produzione
Server: 10.21.12.27 | Istanza: MAGO4 | Azienda: DEMO_Mago4
Utente: Maxidata_1 | Licenza: 0110C137

Apertura connessione a Mago.NET...
Connessione configurata (decommentare OpenConnection per uso reale)

FASE 2: Verifica e creazione clienti
------------------------------------
Controllo esistenza cliente: CLIENTE001
Cliente non esistente - procedura di creazione avviata
Cliente creato: CLIENTE001 - Mario Rossi

FASE 3: Creazione container per le fatture
------------------------------------------
Container mdInvoiceList creato e configurato

FASE 4: Aggiunta fattura di esempio
-----------------------------------
Fattura configurata: FAT001 - Cliente: CLIENTE001 - Importo: EUR 100.00

FASE 5: Aggiunta righe di dettaglio alla fattura
-----------------------------------------------
Riga aggiunta: Prestazione medica - EUR 100.00

FASE 6: Scrittura dati a Mago.NET
---------------------------------
Scrittura fatture in corso...
SCRITTURA COMPLETATA CON SUCCESSO (modalità simulazione)

RIEPILOGO OPERAZIONE COMPLETATA
-------------------------------
Clienti elaborati: 1
Fatture elaborate: 1
Tipo documento: Fattura standard (3407874)
Cliente: CLIENTE001 - Mario Rossi
Importo: EUR 100.00 + IVA

ISTRUZIONI PER L'UTILIZZO IN PRODUZIONE
--------------------------------------
1. Configurare i parametri di connessione nella sezione CONFIGURAZIONE
2. Decommentare connectionHelper.OpenConnection()
3. Decommentare customersList.Scrivi() e invoiceList.Scrivi()
4. Implementare la gestione errori specifica per l'applicazione
5. Testare con dati di prova prima dell'uso in produzione
6. Verificare sempre i risultati delle operazioni di scrittura
```

## Integrazione in Produzione

### 1. Configurazione Parametri
Aggiornare tutti i parametri nella sezione di configurazione:
```vb
' Connessione Mago.NET
Public Const MAGO_SERVER_IP As String = "your_server_ip"
Public Const MAGO_INSTANCE As String = "your_instance"
Public Const MAGO_COMPANY As String = "your_company"
Public Const MAGO_USERNAME As String = "your_username"
Public Const MAGO_PASSWORD As String = "your_password"
Public Const MAGO_LICENSE As String = "your_license"

' Parametri aziendali
Public Const CODICE_PAGAMENTO As String = "your_payment_code"
Public Const REGISTRO_IVA As String = "your_vat_register"
```

### 2. Attivazione Connessione Reale
Decommentare nel metodo Main():
```vb
connectionHelper.OpenConnection()
```

### 3. Attivazione Scrittura Dati
Decommentare le chiamate di scrittura:
```vb
Dim risultatoClienti As Boolean = customersList.Scrivi()
Dim risultatoFatture As Boolean = invoiceList.Scrivi()
```

### 4. Gestione Errori Personalizzata
Implementare la gestione degli errori specifica per l'applicazione:
```vb
If Not risultatoFatture Then
    ' Log errore nel sistema di logging aziendale
    LogError($"Errore fatture {invoiceList.ErroreNumero}: {invoiceList.ErroreMessaggio}")
    ' Notifica amministratori
    SendAlert($"Errore sincronizzazione: {invoiceList.ErroreMessaggio}")
    Return
End If
```

### 5. Integrazione con Database Aziendale
Sostituire i dati hardcoded con query al database:
```vb
' Esempio caricamento clienti da database
Dim clienti = LoadCustomersFromDatabase()
For Each cliente In clienti
    ' Processo di creazione cliente
Next
```

## Gestione Errori

Il programma include gestione completa degli errori Mago.NET:

| Codice | Descrizione |
|--------|-------------|
| 1 | Errore istanziazione LoginManager |
| 3 | Errore autenticazione |
| 4 | Errore parametri TbServices |
| 5 | Errore connessione TbServices |
| 7 | Nessun dato nel database |
| 8 | Errore conversione XML |
| 9 | Errore estrazione dati XML |
| 11 | Errore disconnessione TbServices |
| 12 | Errore logout LoginManager |

## Riferimenti

- **Codice originale**: `Modules/MedLAN.MNET/Data/Sync.vb` (metodo `SyncInvoice`)
- **Documentazione TransferMNET**: `LIB/TransferMNET/`
- **Helper connessioni**: `MagoConnectionHelper.vb`
- **Enumerazioni**: `Enum.vb`