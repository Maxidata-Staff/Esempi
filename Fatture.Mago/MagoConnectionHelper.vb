Imports System
Imports Maxidata.TransferMNET

''' <summary>
''' Classe helper per la gestione delle connessioni a Mago.NET tramite la libreria TransferMNET.
''' 
''' Questa classe fornisce:
''' - Gestione centralizzata delle connessioni al web service Mago.NET
''' - Gestione degli errori con messaggi dettagliati
''' - Metodi di utilità per operazioni comuni
''' - Configurazione automatica dei parametri di logging
''' 
''' Utilizzo tipico:
''' 1. Creare un'istanza con i parametri di connessione
''' 2. Chiamare OpenConnection() per stabilire la connessione
''' 3. Eseguire le operazioni richieste (sync fatture, clienti, etc.)
''' 4. Chiamare CloseConnection() per rilasciare le risorse
''' </summary>
Public Class MagoConnectionHelper

#Region "Proprietà di configurazione"
    
    ''' <summary>Indirizzo IP del server Mago.NET</summary>
    Public Property IP As String
    
    ''' <summary>Nome dell'istanza Mago.NET (es. "MAGO4")</summary>
    Public Property Instance As String
    
    ''' <summary>Codice azienda presente in Mago.NET</summary>
    Public Property Company As String
    
    ''' <summary>Username per l'autenticazione</summary>
    Public Property Username As String
    
    ''' <summary>Password per l'autenticazione</summary>
    Public Property Password As String
    
    ''' <summary>Codice licenza Mago.NET</summary>
    Public Property License As String
    
    ''' <summary>Directory per i file di log del TransferMNET</summary>
    Public Property LogDirectory As String
    
    ''' <summary>
    ''' Indica se la connessione al web service è attiva
    ''' </summary>
    Public ReadOnly Property IsConnected As Boolean
        Get
            ' Verifica se l'oggetto di connessione WSExhibitor è inizializzato
            Return WSExhibitor.ConnWS IsNot Nothing
        End Get
    End Property
    
#End Region

#Region "Costruttori"
    
    ''' <summary>
    ''' Costruttore di base. Inizializza la directory di log predefinita.
    ''' </summary>
    Public Sub New()
        ' Impostazione della directory di log nella cartella temporanea del sistema
        LogDirectory = IO.Path.Combine(My.Computer.FileSystem.SpecialDirectories.Temp, "Mago")
    End Sub
    
    ''' <summary>
    ''' Costruttore con parametri di connessione.
    ''' </summary>
    ''' <param name="ip">Indirizzo IP del server Mago.NET</param>
    ''' <param name="instance">Nome istanza Mago.NET</param>
    ''' <param name="company">Codice azienda</param>
    ''' <param name="username">Username di accesso</param>
    ''' <param name="password">Password di accesso</param>
    ''' <param name="license">Codice licenza</param>
    Public Sub New(ip As String, instance As String, company As String, 
                   username As String, password As String, license As String)
        ' Chiama il costruttore base per inizializzare la directory di log
        Me.New()
        
        ' Assegnazione dei parametri di connessione
        Me.IP = ip
        Me.Instance = instance
        Me.Company = company
        Me.Username = username
        Me.Password = password
        Me.License = license
    End Sub
    
#End Region

#Region "Gestione connessioni"
    
    ''' <summary>
    ''' Apre una nuova connessione al web service Mago.NET.
    ''' 
    ''' Questo metodo:
    ''' 1. Valida tutti i parametri di connessione richiesti
    ''' 2. Crea la directory di log se non esiste
    ''' 3. Inizializza la connessione tramite WSExhibitor
    ''' 4. Configura i parametri di logging per TransferMNET
    ''' </summary>
    ''' <exception cref="ArgumentException">Viene sollevata se mancano parametri obbligatori</exception>
    ''' <exception cref="Exception">Viene sollevata per errori di connessione o configurazione</exception>
    Public Sub OpenConnection()
        Try
            ' === VALIDAZIONE PARAMETRI ===
            ' Verifica che tutti i parametri obbligatori siano presenti
            If String.IsNullOrEmpty(IP) OrElse String.IsNullOrEmpty(Instance) OrElse
               String.IsNullOrEmpty(Company) OrElse String.IsNullOrEmpty(Username) OrElse String.IsNullOrEmpty(License) Then
                Throw New ArgumentException("Errore configurazione: tutti i parametri di connessione sono richiesti (IP, Instance, Company, Username, License)")
            End If

            ' === PREPARAZIONE DIRECTORY LOG ===
            ' Crea la directory per i file di log se non esiste
            If Not IO.Directory.Exists(LogDirectory) Then
                IO.Directory.CreateDirectory(LogDirectory)
            End If
            
            ' === INIZIALIZZAZIONE CONNESSIONE ===
            ' Chiamata al metodo principale di inizializzazione del web service
            Dim risultatoConnessione As Integer = WSExhibitor.InizializzaConnWS(IP, Instance, Company, Username, Password, License)
            
            ' Verifica del risultato e gestione errori tramite metodo centralizzato
            GetError(risultatoConnessione, 0)
            
            ' === CONFIGURAZIONE TRANSFERMNET ===
            ' Configurazione dei parametri specifici per la libreria TransferMNET
            WSExhibitor.CurrentLogDirectory = LogDirectory           ' Directory per i log
            WSExhibitor.DataDiApplicazione = Date.Now               ' Data di riferimento per l'applicazione
            
        Catch ex As Exception
            ' Re-throwing dell'eccezione con informazioni aggiuntive per il debugging
            Throw New Exception($"Errore durante l'apertura della connessione a Mago.NET: {ex.Message}", ex)
        End Try
    End Sub
    
    ''' <summary>
    ''' Chiude la connessione al web service Mago.NET e rilascia le risorse.
    ''' 
    ''' Questo metodo è sicuro da chiamare anche se la connessione non è attiva.
    ''' Gli errori durante la chiusura vengono loggati ma non rilanciano eccezioni
    ''' per permettere la corretta pulizia delle risorse.
    ''' </summary>
    Public Sub CloseConnection()
        Try
            ' Verifica se esiste una connessione attiva prima di tentare la chiusura
            If IsConnected Then
                WSExhibitor.RilasciaConnWS()
            End If
        Catch ex As Exception
            ' Log dell'errore senza rilancio dell'eccezione per permettere la pulizia
            Console.WriteLine($"Avviso durante la chiusura della connessione: {ex.Message}")
        End Try
    End Sub
    
#End Region

#Region "Gestione errori centralizzata"
    
    ''' <summary>
    ''' Metodo centralizzato per la gestione degli errori restituiti da Mago.NET.
    ''' 
    ''' Questo metodo traduce i codici di errore numerici in messaggi descrittivi
    ''' e solleva eccezioni appropriate con informazioni dettagliate per il debugging.
    ''' </summary>
    ''' <param name="errNumber">Codice di errore restituito da Mago.NET</param>
    ''' <param name="returnLogin">Codice di errore aggiuntivo per problemi di login</param>
    ''' <exception cref="Exception">Viene sollevata per tutti i codici di errore diversi da 0</exception>
    Friend Shared Sub GetError(ByVal errNumber As Integer, ByVal returnLogin As Integer)
        ' Se non c'è errore (codice 0), esce senza azioni
        If errNumber = 0 Then Return
        
        ' Traduzione del codice di errore in messaggio descrittivo
        Dim messaggioErrore As String = String.Empty
        Select Case errNumber
            Case 1 
                messaggioErrore = "Impossibile istanziare il Web Service LoginManager, o farne il login." + vbCrLf +
                                "Verificare l'edizione installata e le licenze registrate di Mago.Net"
                                
            Case 3 
                messaggioErrore = "Impossibile effettuare il login al Web Service LoginManager." + vbCrLf +
                                "Errore di autenticazione: " + CStr(returnLogin) + vbCrLf +
                                "Verificare username, password e licenza"
                                
            Case 4 
                messaggioErrore = "Creazione della stringa di parametri da utilizzare con TbServices non riuscita." + vbCrLf +
                                "Problema nella configurazione dei servizi interni"
                                
            Case 5 
                messaggioErrore = "Impossibile istanziare il Web Service TbServices, GetData non riuscito." + vbCrLf +
                                "Caricamento dei dati non effettuato." + vbCrLf +
                                "Verificare la connessione di rete e la disponibilità del server"
                                
            Case 7 
                messaggioErrore = "Nessun dato letto dal database di Mago.Net" + vbCrLf +
                                "Verificare di aver installato correttamente i profili di esportazione" + vbCrLf +
                                "e che vi siano inseriti i dati di base nell'azienda corrente"
                                
            Case 8 
                messaggioErrore = "Errore nella conversione dei dati letti in formato XML." + vbCrLf +
                                "Problema nella serializzazione/deserializzazione dei dati"
                                
            Case 9 
                messaggioErrore = "Errore nell'estrazione dati dal nodo XML letto da TbServices." + vbCrLf +
                                "Struttura dati XML non conforme alle aspettative"
                                
            Case 10 
                messaggioErrore = "Errore durante la lettura del campo codice di prova." + vbCrLf +
                                "Problema nell'accesso ai dati di configurazione"
                                
            Case 11 
                messaggioErrore = "Errore alla disconnessione dal Web Service TbServices." + vbCrLf +
                                "La connessione potrebbe essere già stata chiusa"
                                
            Case 12 
                messaggioErrore = "Errore durante il LogOff dal Web Service LoginManager." + vbCrLf +
                                "Problema nella procedura di logout"
                                
            Case Else
                messaggioErrore = $"Errore sconosciuto con codice: {errNumber}"
        End Select

        ' Aggiunta di informazioni per il supporto tecnico
        messaggioErrore += vbCrLf + vbCrLf + "Per ulteriore assistenza, contattare il supporto tecnico fornendo:"
        messaggioErrore += vbCrLf + "- Codice errore: " + errNumber.ToString()
        messaggioErrore += vbCrLf + "- Parametri di connessione utilizzati"
        messaggioErrore += vbCrLf + "- Log dell'applicazione"
        
        ' Sollevamento dell'eccezione con il messaggio completo
        Throw New Exception(messaggioErrore)
    End Sub
    
#End Region

#Region "Metodi di utilità"
    
    ''' <summary>
    ''' Testa la connessione con i parametri specificati senza mantenere la connessione aperta.
    ''' 
    ''' Utile per verificare la correttezza dei parametri di configurazione
    ''' prima di avviare operazioni più complesse.
    ''' </summary>
    ''' <returns>True se la connessione è riuscita, False altrimenti</returns>
    Public Function TestConnection() As Boolean
        Try
            ' Tentativo di apertura e chiusura immediata della connessione
            OpenConnection()
            CloseConnection()
            Return True
        Catch
            ' In caso di errore, restituisce False senza sollevare eccezioni
            Return False
        End Try
    End Function
    
    ''' <summary>
    ''' Verifica se un cliente esiste già nel database Mago.NET.
    ''' 
    ''' NOTA: Questo è un metodo di esempio. In un'implementazione reale,
    ''' dovrebbe contenere la logica per interrogare il database Mago.NET
    ''' e verificare l'esistenza del cliente specificato.
    ''' </summary>
    ''' <param name="customerCode">Codice cliente da verificare</param>
    ''' <returns>True se il cliente esiste, False altrimenti</returns>
    ''' <exception cref="InvalidOperationException">Viene sollevata se non c'è una connessione attiva</exception>
    ''' <exception cref="Exception">Viene sollevata per errori durante la verifica</exception>
    Public Function CustomerExists(customerCode As String) As Boolean
        Try
            ' Verifica prerequisiti
            If Not IsConnected Then
                Throw New InvalidOperationException("Connessione al web service non attiva. Chiamare OpenConnection() prima di questa operazione.")
            End If
            
            ' === IMPLEMENTAZIONE DA PERSONALIZZARE ===
            ' In un'implementazione reale, qui andrebbe inserita la logica per:
            ' 1. Interrogare il database Mago.NET tramite i web service disponibili
            ' 2. Cercare il cliente con il codice specificato
            ' 3. Restituire True se trovato, False altrimenti
            
            ' Per questo esempio, restituisce sempre False per semplicità
            Return False
            
        Catch ex As Exception
            ' Re-throwing con informazioni aggiuntive per il debugging
            Throw New Exception($"Errore durante la verifica del cliente '{customerCode}': {ex.Message}", ex)
        End Try
    End Function
    
#End Region

End Class
