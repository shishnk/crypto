namespace Crypto_1_OOP_
{
    interface INGrams
    {
        void ReadFromFile(string path); // чтение эталонных Nграмм с файла
        void GetFromText(string text); // чтение Nграмм с зашифрованного текста
        void SortingByDescending(); // сортировка Nграмм по убыванию частоты
    }
}
