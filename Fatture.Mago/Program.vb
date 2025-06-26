Imports System
Imports Maxidata.TransferMNET

''' <summary>
''' Programma di esempio per dimostrare l'utilizzo della libreria Maxidata.TransferMNET.dll
''' per la sincronizzazione delle fatture con Mago.NET.
''' 
''' Questo esempio replica la logica essenziale del metodo SyncInvoice utilizzando:
''' - La classe MagoConnectionHelper per la gestione delle connessioni
''' - Il pattern mdInvoiceList per la creazione delle fatture
''' - Il pattern mdCustomersList per la gestione dei clienti
''' - La gestione degli errori centralizzata
''' 
''' IMPORTANTE: Prima dell'utilizzo in produzione, configurare tutti i parametri
''' nella sezione "PARAMETRI DI CONFIGURAZIONE" indicata di seguito.
''' </summary>
Module Program

    '==============================================================================
    ' PARAMETRI DI CONFIGURAZIONE - PERSONALIZZAZIONE RICHIESTA
    '==============================================================================
    ' Questi parametri devono essere configurati secondo l'ambiente di destinazione
    ' prima dell'utilizzo in produzione.
    
    ' >>> CONFIGURAZIONE CONNESSIONE MAGO.NET <<<
    ' Modificare questi valori con i parametri reali del server Mago.NET
    Public Const MAGO_SERVER_IP As String = "10.21.12.27"              ' Indirizzo IP del server Mago.NET
    Public Const MAGO_INSTANCE As String = "MAGO4"                     ' Nome istanza Mago.NET
    Public Const MAGO_COMPANY As String = "DEMO_Mago4"                 ' Codice azienda
    Public Const MAGO_USERNAME As String = "Maxidata_1"               ' Username per l'accesso
    Public Const MAGO_PASSWORD As String = ""                         ' Password per l'accesso
    Public Const MAGO_LICENSE As String = "0110C137"                  ' Codice licenza

    ' >>> CONFIGURAZIONE PARAMETRI AZIENDALI <<<
    ' Questi codici devono corrispondere alla configurazione presente in Mago.NET
    Public Const CLIENTE_CONTO_PREDEFINITO As String = "01011000"     ' Conto contabile clienti di default
    Public Const CODICE_PAGAMENTO As String = "CONT"                  ' Codice condizioni di pagamento
    Public Const REGISTRO_IVA As String = "VEN"                       ' Registro IVA di vendita
    Public Const MODELLO_CONTABILE As String = "FE"                   ' Modello contabile per fatture
    Public Const CODICE_IVA_STANDARD As String = "22"                 ' Codice IVA per aliquota 22%
    
    ' >>> CONFIGURAZIONE DOCUMENTI <<<
    ' Codici per la tipologia di documenti da creare
    Public Const TIPO_DOCUMENTO_FATTURA As String = "22151169"        ' Tipo documento fattura elettronica

    
    '==============================================================================

    ''' <summary>
    ''' Metodo principale del programma di esempio.
    ''' Dimostra il processo completo di sincronizzazione delle fatture con Mago.NET:
    ''' 1. Configurazione e apertura della connessione
    ''' 2. Verifica e creazione clienti
    ''' 3. Creazione del container per le fatture
    ''' 4. Aggiunta di fatture di esempio con dettagli
    ''' 5. Scrittura definitiva dei dati a Mago.NET
    ''' 6. Chiusura della connessione e pulizia risorse
    ''' </summary>
    Sub Main()
        Console.WriteLine("=== PROGRAMMA DI ESEMPIO - TransferMNET ===")
        Console.WriteLine("Sincronizzazione fatture con Mago.NET")
        Console.WriteLine(New String("="c, 50))
        Console.WriteLine()

        ' Variabile per la gestione della connessione
        Dim connectionHelper As MagoConnectionHelper = Nothing

        Try
            '==========================================================================
            ' FASE 1: CONFIGURAZIONE E APERTURA CONNESSIONE
            '==========================================================================
            Console.WriteLine("FASE 1: Configurazione connessione a Mago.NET")
            Console.WriteLine("--------------------------------------------------")

            ' Creazione dell'helper per la gestione della connessione
            ' NOTA: I parametri di connessione sono definiti nelle costanti in cima al file
            connectionHelper = New MagoConnectionHelper(MAGO_SERVER_IP, MAGO_INSTANCE, MAGO_COMPANY,
                                                       MAGO_USERNAME, MAGO_PASSWORD, MAGO_LICENSE)

            Console.WriteLine("ATTENZIONE: Configurare i parametri di connessione reali prima dell'uso in produzione")
            Console.WriteLine($"Server: {MAGO_SERVER_IP} | Istanza: {MAGO_INSTANCE} | Azienda: {MAGO_COMPANY}")
            Console.WriteLine($"Utente: {MAGO_USERNAME} | Licenza: {MAGO_LICENSE}")
            Console.WriteLine()

            ' Apertura della connessione a Mago.NET
            Console.WriteLine("Apertura connessione a Mago.NET...")
            ' NOTA: Decommentare la riga seguente per attivare la connessione reale
            connectionHelper.OpenConnection()
            Console.WriteLine("Connessione configurata (decommentare OpenConnection per uso reale)")
            Console.WriteLine()

            '==========================================================================
            ' FASE 2: GESTIONE CLIENTI
            '==========================================================================
            Console.WriteLine("FASE 2: Verifica e creazione clienti")
            Console.WriteLine("------------------------------------")

            ' Creazione del container per i clienti
            Dim customersList As New mdCustomersList()
            customersList.NomeTabella = "EsempioClienti"

            ' Definizione del cliente di esempio
            ' PERSONALIZZAZIONE: Modificare questi dati con i clienti reali
            Dim codiceCliente As String = "CLIENTE001"
            Console.WriteLine($"Controllo esistenza cliente: {codiceCliente}")

            ' Verifica se il cliente esiste già nel database
            Dim clienteEsiste As Boolean = False
            If connectionHelper.IsConnected Then
                clienteEsiste = connectionHelper.CustomerExists(codiceCliente)
            End If

            If Not clienteEsiste Then
                Console.WriteLine("Cliente non esistente - procedura di creazione avviata")

                ' Aggiunta del nuovo cliente utilizzando il pattern addCustomer
                ' Questo codice replica la logica del metodo addCustumer originale
                customersList.AggiungiVoceDati(customersList.NuovaVoceDati)
                With customersList.VoceAttuale.Data.CustomersSuppliers
                    .CustSuppType = New Maxidata_Customers.CustomersSuppliersCustSuppType
                    .CustSuppType.Value = CInt(eMago_CustSuppType.CLIENTE).ToString
                    .CustSupp.Value = codiceCliente
                    ' PERSONALIZZAZIONE: Modificare i dati del cliente secondo le necessità
                    .CompanyName = "Mario Rossi"                    ' Ragione sociale
                    .TaxIdNumber = "12345678901"                    ' Partita IVA
                    .FiscalCode = "RSSMRA80A01H501X"               ' Codice fiscale
                    .Account = CLIENTE_CONTO_PREDEFINITO           ' Conto contabile
                    .Address = "Via Roma 123"                      ' Indirizzo
                    .ZIPCode = "00100"                             ' CAP
                    .City = "Roma"                                 ' Città
                    .County = "RM"                                 ' Provincia
                    .Region = "Lazio"                              ' Regione
                    .Country = "Italia"                            ' Nazione
                    .Telephone1 = "06123456"                       ' Telefono
                    .EMail = "mario.rossi@email.it"                ' Email
                    .ISOCountryCode = "IT"                         ' Codice paese ISO
                    .CustSuppKind = New Maxidata_Customers.CustomersSuppliersCustSuppKind
                    .CustSuppKind.Value = CStr(eMago_CustSuppKind.Nazionale)
                    .TBModified = Date.Now.ToString("yyyy-MM-dd HH:mm:ss")
                    ' Impostazione persona fisica a Nothing come da specifica originale
                    .NaturalPerson = Nothing
                End With

                Console.WriteLine($"Cliente creato: {codiceCliente} - Mario Rossi")
            Else
                Console.WriteLine("Cliente già esistente - salto la fase di creazione")
            End If
            Console.WriteLine()

            '==========================================================================
            ' FASE 3: CREAZIONE CONTAINER FATTURE
            '==========================================================================
            Console.WriteLine("FASE 3: Creazione container per le fatture")
            Console.WriteLine("------------------------------------------")

            ' Creazione del container principale per le fatture (mdInvoiceList)
            ' Questo oggetto gestirà tutte le fatture da sincronizzare
            Dim invoiceList As New mdInvoiceList()
            invoiceList.NomeTabella = "EsempioFatture"
            Console.WriteLine("Container mdInvoiceList creato e configurato")
            Console.WriteLine()

            '==========================================================================
            ' FASE 4: CREAZIONE E CONFIGURAZIONE FATTURA
            '==========================================================================
            Console.WriteLine("FASE 4: Aggiunta fattura di esempio")
            Console.WriteLine("-----------------------------------")

            ' Aggiunta di una nuova fattura al container
            ' Il parametro specifica il tipo di documento (Fattura standard)
            invoiceList.AggiungiVoceDati(invoiceList.NuovaVoceDati(CInt(eMago_DocumentType.Fattura).ToString))

            ' Configurazione dei dati principali della fattura
            With invoiceList.VoceAttuale.Data.SaleDocument
                .DocumentType.Value = CInt(eMago_DocumentType.Fattura).ToString
                .DocNo = "FAT001"                                   ' Numero progressivo fattura
                .DocumentDate = Date.Now.ToString("yyyy-MM-dd")     ' Data documento
                .DocumentDateSpecified = True
                .EIDocumentType = New Maxidata_Invoice.SaleDocumentEIDocumentType
                .EIDocumentType.Value = TIPO_DOCUMENTO_FATTURA      ' Tipo fattura elettronica
                .CustSuppType.Value = CInt(eMago_CustSuppType.CLIENTE).ToString
                .CustSupp = codiceCliente                           ' Collegamento al cliente
                .Payment = CODICE_PAGAMENTO                         ' Condizioni di pagamento
                .PostingDate = Date.Now.ToString("yyyy-MM-dd")      ' Data di registrazione
                .PostingDateSpecified = True
                .AccTpl = MODELLO_CONTABILE                         ' Modello contabile
                .TaxJournal = REGISTRO_IVA                          ' Registro IVA
                .Issued = "True"                                    ' Fattura emessa
                .TBModified = Date.Now.ToString("yyyy-MM-dd HH:mm:ss")
            End With

            ' Configurazione degli importi della fattura
            With invoiceList.VoceAttuale.Data.Charges
                .ServiceAmounts = 100.0                             ' Imponibile servizi
                .ServiceAmountsSpecified = True
                .TotalAmount = 122.0                                ' Totale con IVA (100 + 22% IVA)
                .TotalAmountSpecified = True
                .PayableAmount = 122.0                              ' Importo da pagare
                .PayableAmountSpecified = True
                .FreeSamples = 0                                    ' Omaggi
                .FreeSamplesSpecified = True
                .StampsCharges = 0                                  ' Bolli
                .StampsChargesSpecified = True
                .TBModified = Date.Now.ToString("yyyy-MM-dd HH:mm:ss")
            End With

            Console.WriteLine($"Fattura configurata: FAT001 - Cliente: {codiceCliente} - Importo: EUR 100.00")

            '==========================================================================
            ' FASE 5: AGGIUNTA RIGHE DI DETTAGLIO
            '==========================================================================
            Console.WriteLine("FASE 5: Aggiunta righe di dettaglio alla fattura")
            Console.WriteLine("-----------------------------------------------")

            ' Aggiunta di una riga di dettaglio alla fattura
            Dim rigaIndex As Integer = 0
            If invoiceList.AggiungiDetailRow(rigaIndex) Then
                ' Configurazione della riga di dettaglio
                With invoiceList.VoceAttuale.Data.Detail.DetailRow(rigaIndex)
                    .DocumentType.Value = CInt(eMago_DocumentType.Fattura).ToString
                    .DocumentDate = invoiceList.VoceAttuale.Data.SaleDocument.DocumentDate
                    .DocumentDateSpecified = True
                    .CustSuppType.Value = CInt(eMago_CustSuppType.CLIENTE).ToString
                    .CustSupp = codiceCliente                       ' Cliente di riferimento
                    .SubId = CShort(rigaIndex + 1)                  ' Numero progressivo riga
                    .SubIdSpecified = True
                    .Line.Value = CShort(rigaIndex + 1)             ' Numero riga
                    .LineType.Value = CInt(eMago_LineType.Servizio).ToString()
                    ' PERSONALIZZAZIONE: Modificare descrizione e importi secondo necessità
                    .Description = "Prestazione medica specialistica"      ' Descrizione prestazione
                    .Qty = 1                                        ' Quantità
                    .QtySpecified = True
                    .UnitValue = 100.0                              ' Prezzo unitario
                    .UnitValueSpecified = True
                    .TaxableAmount = 100.0                          ' Imponibile
                    .TaxableAmountSpecified = True
                    .TotalAmount = 100.0                            ' Totale riga
                    .TotalAmountSpecified = True
                    .TaxCode = CODICE_IVA_STANDARD                  ' Codice IVA
                    .TBModified = Date.Now.ToString("yyyy-MM-dd HH:mm:ss")
                End With
                Console.WriteLine("Riga aggiunta: Prestazione medica - EUR 100.00")
            End If
            Console.WriteLine()

            '==========================================================================
            ' FASE 6: SCRITTURA DEFINITIVA A MAGO.NET
            '==========================================================================
            Console.WriteLine("FASE 6: Scrittura dati a Mago.NET")
            Console.WriteLine("---------------------------------")

            Try
                ' Scrittura dei clienti se necessario
                If customersList.Count > 0 Then
                    Console.WriteLine("Scrittura clienti in corso...")
                    Dim risultatoClienti As Boolean = customersList.Scrivi()
                    If Not risultatoClienti Then
                        Console.WriteLine($"ERRORE clienti {customersList.ErroreNumero}: {customersList.ErroreMessaggio}")
                        Return
                    End If
                End If

                ' Scrittura delle fatture
                Console.WriteLine("Scrittura fatture in corso...")
                Dim risultatoFatture As Boolean = invoiceList.Scrivi()
                If Not risultatoFatture Then
                    Console.WriteLine($"ERRORE fatture {invoiceList.ErroreNumero}: {invoiceList.ErroreMessaggio}")
                    Return
                End If

            Catch ex As Exception
                Console.WriteLine($"ERRORE durante la scrittura: {ex.Message}")
                Throw
            End Try
            Console.WriteLine()

            '==========================================================================
            ' RIEPILOGO OPERAZIONE
            '==========================================================================
            Console.WriteLine("RIEPILOGO OPERAZIONE COMPLETATA")
            Console.WriteLine("-------------------------------")
            Console.WriteLine($"Clienti elaborati: {customersList.Count}")
            Console.WriteLine($"Fatture elaborate: {invoiceList.Count}")
            Console.WriteLine($"Tipo documento: Fattura standard (3407874)")
            Console.WriteLine($"Cliente: {codiceCliente} - Mario Rossi")
            Console.WriteLine($"Importo: EUR 100.00 + IVA")
            Console.WriteLine()

        Catch ex As Exception
            '==========================================================================
            ' GESTIONE ERRORI GLOBALE
            '==========================================================================
            Console.WriteLine("ERRORE DURANTE L'ESECUZIONE")
            Console.WriteLine("---------------------------")
            Console.WriteLine($"Messaggio: {ex.Message}")
            Console.WriteLine()
            Console.WriteLine("Dettagli tecnici:")
            Console.WriteLine(ex.ToString())
            
        Finally
            '==========================================================================
            ' PULIZIA RISORSE E CHIUSURA CONNESSIONE
            '==========================================================================
            Console.WriteLine()
            Console.WriteLine("CHIUSURA CONNESSIONE E PULIZIA RISORSE")
            Console.WriteLine("-------------------------------------")
            If connectionHelper IsNot Nothing Then
                connectionHelper.CloseConnection()
                Console.WriteLine("Connessione chiusa correttamente")
            End If
            
            Console.WriteLine()
            Console.WriteLine("Operazione terminata. Premere un tasto per uscire...")
            Console.ReadKey()
        End Try
    End Sub

End Module
