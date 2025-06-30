''' <summary>
''' Modulo contenente le costanti e le enumerazioni utilizzate da Mago.NET per TransferMNET.
''' 
''' Questo file contiene:
''' - Costanti per i tipi di riferimento e configurazioni standard
''' - Enumerazioni per tutti i valori numerici utilizzati da Mago.NET
''' - Codici per documenti, clienti, fornitori, pagamenti e altre entità
''' 
''' IMPORTANTE: Questi valori devono corrispondere esattamente ai codici
''' configurati nel database Mago.NET dell'azienda di destinazione.
''' </summary>
Public Module Constants

    '==========================================================================
    ' COSTANTI PER TIPI DI RIFERIMENTO E CONFIGURAZIONI STANDARD
    '==========================================================================
    ' Queste costanti definiscono i comportamenti standard per vari aspetti
    ' del sistema Mago.NET. Non modificare senza consultare la documentazione.

    ''' <summary>Tipo di stampa standard per i riferimenti</summary>
    Public Const eMago_ReferencesPrintType_STANDARD = "524293"

    ''' <summary>Selezione lotti standard</summary>
    Public Const eMago_LotSelection_STANDARD = "8454147"

    ''' <summary>Addebito IVA su omaggi</summary>
    Public Const eMago_DebitFreeSamplesTaxAmount_IVASUOMAGGI = "3276802"

    ''' <summary>Segno contabile: Dare</summary>
    Public Const eMago_SegnoContabile_DARE = "4980736"

    ''' <summary>Segno contabile: Avere</summary>
    Public Const eMago_SegnoContabile_AVERE = "4980737"

    ''' <summary>Presentazione: Salvo buon fine</summary>
    Public Const eMago_Presentation_SALVOBUONFINE = "1376256"

    ''' <summary>Maturazione: Sul fatturato</summary>
    Public Const eMago_Maturazione_SULFATTURATO = "3473408"

    ''' <summary>Transazione: Acquisto/Vendita</summary>
    Public Const eMago_Transaction_ACQVEND = "5767168"

    ''' <summary>Condizioni di consegna: Franco fabbrica</summary>
    Public Const eMago_DeliveryTerms_FRANCOFABBRICA = "5963781"

    ''' <summary>Modalità di trasporto: Stradale</summary>
    Public Const eMago_ModeOfTransport_STRADALE = "5832706"

    ''' <summary>Operazione: Acquisto finale</summary>
    Public Const eMago_Operation_ACQFINALE = "5898242"

End Module

'==============================================================================
' ENUMERAZIONI PER TIPI DI ENTITÀ E CONFIGURAZIONI
'==============================================================================

''' <summary>
''' Definisce il tipo di soggetto: Cliente o Fornitore
''' Utilizzato per distinguere tra clienti e fornitori nell'anagrafica
''' </summary>
Public Enum eMago_CustSuppType
    ''' <summary>Soggetto di tipo Cliente</summary>
    CLIENTE = 3211264
    ''' <summary>Soggetto di tipo Fornitore</summary>
    FORNITORE = 3211265
End Enum

''' <summary>
''' Definisce la tipologia fiscale del cliente/fornitore
''' Importante per la gestione dell'IVA e delle dichiarazioni fiscali
''' </summary>
Public Enum eMago_CustSuppKind
    ''' <summary>Cliente/Fornitore nazionale (Italia)</summary>
    Nazionale = 7733248
    ''' <summary>Cliente/Fornitore della Comunità Europea</summary>
    CEE = 7733249
    ''' <summary>Cliente/Fornitore extra-comunitario</summary>
    ExtraCEE = 7733250
End Enum

''' <summary>
''' Definisce il genere per persone fisiche
''' </summary>
Public Enum eMago_Gender
    ''' <summary>Persona di genere maschile</summary>
    Maschile = 2097152
    ''' <summary>Persona di genere femminile</summary>
    Femminile = 2097153
End Enum

''' <summary>
''' Tipologia specifica per fornitori (equivalente a CustSuppKind)
''' Mantenuto per compatibilità con versioni precedenti
''' </summary>
Public Enum eMago_SupplierType
    ''' <summary>Fornitore italiano</summary>
    Italiano = 7733248
    ''' <summary>Fornitore della Comunità Europea</summary>
    CEE = 7733249
    ''' <summary>Fornitore extra-comunitario</summary>
    ExtraCEE = 7733250
End Enum

'==============================================================================
' ENUMERAZIONI PER MODALITÀ DI PAGAMENTO
'==============================================================================

''' <summary>
''' Definisce le modalità di pagamento disponibili per le rate
''' Utilizzato nella configurazione delle scadenze di pagamento
''' </summary>
Public Enum eMago_InstallmentType
    ''' <summary>Pagamento in contanti</summary>
    Contante = 2686976
    ''' <summary>Rimessa diretta</summary>
    RimDir = 2686977
    ''' <summary>Pagamento in contrassegno</summary>
    Contrassegno = 2686978
    ''' <summary>Pagamento tramite cambiale</summary>
    Cambiale = 2686980
    ''' <summary>Ricevuta bancaria (RiBa)</summary>
    RiBa = 2686981
    ''' <summary>Bonifico bancario</summary>
    Bonifico = 2686982
    ''' <summary>Pagamento con assegno</summary>
    Assegno = 2686983
    ''' <summary>Assegno circolare</summary>
    AssegnoCircolare = 2686984
    ''' <summary>RID SEPA (addebito diretto)</summary>
    RidSepa = 2686989
    ''' <summary>Carta di credito</summary>
    CartaCredito = 2686992
    ''' <summary>Bancomat/POS</summary>
    Bancomat = 2686998
End Enum

''' <summary>
''' Definisce come l'IVA viene distribuita tra le rate di pagamento
''' </summary>
Public Enum eMago_TaxInstallment
    ''' <summary>IVA distribuita tra tutte le rate</summary>
    TraLeRate = 2752515
    ''' <summary>IVA interamente nella prima rata</summary>
    PrimaRata = 2752512
    ''' <summary>IVA in rata separata</summary>
    SoloIva = 2752514
End Enum

''' <summary>
''' Definisce il criterio per calcolare la data di scadenza delle rate
''' </summary>
Public Enum eMago_DueDateType
    ''' <summary>Data da indicare manualmente</summary>
    DaIndicare = 2949120
    ''' <summary>Calcolo basato sulla data fattura</summary>
    DataFattura = 2949121
    ''' <summary>Calcolo a fine mese</summary>
    FineMese = 2949122
End Enum

''' <summary>
''' Modalità di arrotondamento per gli importi
''' </summary>
Public Enum eMago_RoundingType
    ''' <summary>Nessun arrotondamento (default)</summary>
    Nessuno = 786432
    ''' <summary>Arrotondamento per eccesso</summary>
    Eccesso = 786433
    ''' <summary>Arrotondamento per difetto</summary>
    Difetto = 786434
    ''' <summary>Arrotondamento matematico (valore assoluto)</summary>
    Matematico = 786435
    ''' <summary>Arrotondamento matematico con segno</summary>
    Matematicoconsegno = 786436
End Enum

'==============================================================================
' ENUMERAZIONI PER TIPI DI DOCUMENTO
'==============================================================================

''' <summary>
''' Tipi di documento per documenti di acquisto
''' </summary>
Public Enum eMago_DocumentAcqType
    ''' <summary>Fattura di acquisto</summary>
    Fattura = 9830401
    ''' <summary>Nota di credito ricevuta</summary>
    NotaCreditoRicevuta = 9830402
End Enum

''' <summary>
''' Tipi di documento per documenti di vendita
''' Utilizzato per specificare il tipo di documento da creare
''' </summary>
Public Enum eMago_DocumentType
    ''' <summary>Fattura di vendita standard</summary>
    Fattura = 3407874
    ''' <summary>Fattura accompagnatoria (con DDT)</summary>
    FatturaAccompagnatoria = 3407875
    ''' <summary>Nota di credito/accredito</summary>
    NotaAccredito = 3407876
    ''' <summary>Nota di debito</summary>
    NotaDebito = 3407889
    ''' <summary>Ricevuta fiscale</summary>
    RicevutaFiscale = 3407878
    ''' <summary>Fattura di acconto</summary>
    Acconto = 3407883
End Enum

''' <summary>
''' Tipologia di vendita per la gestione di omaggi e promozioni
''' </summary>
Public Enum eMago_SaleType
    ''' <summary>Omaggio senza imponibile</summary>
    Omaggio = 3670016
    ''' <summary>Omaggio con imponibile</summary>
    Omaggio_imponibile = 3670017
    ''' <summary>Vendita promozionale</summary>
    Promozione = 3670018
    ''' <summary>Sconto merce</summary>
    Sconto_merce = 3670019
    ''' <summary>Vendita normale (default)</summary>
    Normale = 3670020
End Enum

'==============================================================================
' ENUMERAZIONI PER RIGHE DI DETTAGLIO
'==============================================================================

''' <summary>
''' Tipi di riga per i dettagli dei documenti
''' Definisce la natura della riga nel documento
''' </summary>
Public Enum eMago_LineType
    ''' <summary>Riga di tipo nota/commento</summary>
    Nota = 3538944
    ''' <summary>Riga di riferimento</summary>
    Riferimento = 3538945
    ''' <summary>Riga per servizi/prestazioni</summary>
    Servizio = 3538946
    ''' <summary>Riga per merci/prodotti (default)</summary>
    Merce = 3538947
    ''' <summary>Riga puramente descrittiva</summary>
    Descrittiva = 3538948
End Enum

''' <summary>
''' Natura dell'articolo per la gestione dell'inventario
''' </summary>
Public Enum eMago_Nature
    ''' <summary>Articolo semilavorato</summary>
    Semilavorato = 22413312
    ''' <summary>Prodotto finito</summary>
    Prodottofinito = 22413313
    ''' <summary>Articolo di acquisto (default)</summary>
    Acquisto = 22413314
End Enum

'==============================================================================
' ENUMERAZIONI PER GESTIONE CONTABILE E PARTITE
'==============================================================================

''' <summary>
''' Modalità di chiusura delle partite contabili
''' </summary>
Public Enum eMago_ClosingType
    ''' <summary>Chiusura normale</summary>
    Normale = 6946816
    ''' <summary>Chiusura per abbuono</summary>
    Abbuono = 6946817
    ''' <summary>Riapertura partita</summary>
    Riapertura = 6946818
    ''' <summary>Differenza cambio</summary>
    DiffCambio = 6946819
    ''' <summary>Per eccedenza</summary>
    Pereccedenza = 6946820
    ''' <summary>Ritenuta</summary>
    Ritenuta = 6946821
End Enum

''' <summary>
''' Specie di archivio per la classificazione dei documenti
''' </summary>
Public Enum eMago_SpecieArchivio
    ''' <summary>Documento di vendita</summary>
    DocVendita = 3801088
    ''' <summary>Documento di acquisto</summary>
    DocAcquisto = 3801108
End Enum

''' <summary>
''' Tipologia di rata per la gestione delle scadenze
''' </summary>
Public Enum eMago_TipoRata
    ''' <summary>Rata di apertura</summary>
    APERTURA = 5505024
    ''' <summary>Rata di chiusura</summary>
    CHIUSURA = 5505025
End Enum

'==============================================================================
' ENUMERAZIONI PER DICHIARAZIONI E OPERAZIONI SPECIALI
'==============================================================================

''' <summary>
''' Tipologia di dichiarazione per operazioni speciali
''' Utilizzato per dichiarazioni doganali e operazioni intracomunitarie
''' </summary>
Public Enum eMago_DeclType
    ''' <summary>Operazione singola</summary>
    SingleOperation = 1507328
    ''' <summary>Importo limite</summary>
    LimitAmount = 1507329
    ''' <summary>Da data a data (default)</summary>
    FromDateToDate = 1507330
End Enum
