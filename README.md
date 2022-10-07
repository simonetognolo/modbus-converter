# Modbus converter

Il protocollo Modbus supporta canali di comunicazione in bit o in word a 16 bit. Nel caso dei registri di word, i valori rappresentati nativamente sono di tipo `short` con valori che vanno da -32768 a 32767. Nel caso si vogliano rappresentare altri valori (ad es. `ushort`, `int`, `char`) è necessario convertire i valori letti dai registri, tenendo in considerazione la dimensione in byte del formato di destinazione. Ad esempio:

* il formato `int` è composto da 32 bit, quindi vanno prese due registri del canale Modbus;
* il formato `char` è composto da 8 bit, quindi un singolo registro può contenere due valori.

Questo progetto ha due scopi:

1.  fornire un tool per la conversione dei parametri di un canale Modbus nei formati indicati;
2.  illustrare la logica che viene usata per la conversione dei valori nativi.

## Uso del tool

Dati due registri contingui su canale Modbus, si intende il primo come valore _basso_ mentre il secondo come valore _alto_. Inserendo i valori nei rispettivi campi di input il programma unisce i 16 bit dei due valori per creare un array di 32 bit, per poi fare le seguenti conversioni:

* Long (1 `int` a 32 bit);
* ULong (1 `uint` a 32 bit);
* ASCII (4 `char` a 8 bit) (se un singolo valore non è convertibile in un carattere stampabile, il suo valore verrà ignorato).

![image](https://user-images.githubusercontent.com/93708281/194342639-9eb13c33-82db-4f9f-b879-33c75fe682ae.png)

## Logica di conversione

I valori vengono convertiti dal metodo [`EvalValues`](https://github.com/simonetognolo/modbus-converter/blob/main/MainWindow.xaml.cs#L23).
