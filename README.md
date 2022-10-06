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
* ASCII (4 `char` a 8 bit) (se un singolo valore non è convertibile in un carattere stampabile, il suo valore verrà ignorato).

![image](https://user-images.githubusercontent.com/93708281/194260659-a89f4eb7-70b0-4b2f-b3c4-cc23705d423d.png)

## Logica di conversione

I valori vengono convertiti dal metodo `EvalValues`

```cs
private void EvalValues()
{
    if (TbLong == null)
        return;

    if (TbHighValue ==  null || !int.TryParse(TbHighValue.Text, out var highValue))
        highValue = 0;

    if (TbLowValue == null || !int.TryParse(TbLowValue.Text, out var lowValue))
        lowValue = 0;

    // Long result
    var merge = unchecked(BitConverter.IsLittleEndian
        ? (ushort)highValue << 16 | (ushort)lowValue
        : (ushort)lowValue << 16 | (ushort)highValue);

    TbLong.Text = merge.ToString();

    // ASCII result
    var lowAscii = unchecked(BitConverter.GetBytes((ushort)lowValue));
    var highAscii = unchecked(BitConverter.GetBytes((ushort)highValue));

    TbAscii.Text = System.Text.Encoding.ASCII.GetString(lowAscii) + " - " + System.Text.Encoding.ASCII.GetString(highAscii);
}
```
