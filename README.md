# Практические задания по дисциплине "Основы криптографии" #
## Описание заданий для практических работ ##

**1**. Цель: знакомство с элементарными методами шифрования данных и криптоанализа.  
>
**Часть 1. Шифрование методом простой замены**
+ Разработать программу для генерации ключа, путем случайного перемешивания алфавита ([Тасование Фишера — Йетса](https://ru.wikipedia.org/wiki/%D0%A2%D0%B0%D1%81%D0%BE%D0%B2%D0%B0%D0%BD%D0%B8%D0%B5_%D0%A4%D0%B8%D1%88%D0%B5%D1%80%D0%B0_%E2%80%94_%D0%99%D0%B5%D1%82%D1%81%D0%B0)). На выходе должен быть файл с перестановкой алфавита (33 различных русских буквы). 
+ Разработать программу для шифрования текста методом простой замены ([Шифр простой замены](https://ru.wikipedia.org/wiki/%D0%A8%D0%B8%D1%84%D1%80_%D0%BF%D1%80%D0%BE%D1%81%D1%82%D0%BE%D0%B9_%D0%B7%D0%B0%D0%BC%D0%B5%D0%BD%D1%8B)). Программа должна читать ключ и шифруемый текст из файла, удалять из текста все символы кроме русских букв (включая пробелы), приводить текст к нижнему регистру и осуществлять замену в соответствии с ключем. На выходе должен быть файл с зашифрованным текстом. Для всех файлов использовать кодировку UTF-8.
+ Сгенерировать два ключа, зашифровать ими два текста и выложить результаты шифрования в общую папку. Первый текст - написать эссе объемом 200-300 символов (самостоятельно, [Эссе](https://ru.wikipedia.org/wiki/%D0%AD%D1%81%D1%81%D0%B5)), в конце текста должна быть подпись (ФИО авторов). Второй текст - произвольный художественный текст объемом 10 +/- 1 тысяч символов.
>
 **Часть 2**. 
+ Расшифровать короткий текст бригады с ближайшим меньшим номером. Процесс расшифровки подробно описать (приводя шаги и промежуточные результаты).
>
**Часть 3. (дополнительное задание повышенной сложности)**. 
+ Разработать программу способную автоматически расшифровать любой большой текст выложенный другой бригадой.
---
**2**. Цель: знакомство с ассиметричными криптографическими алгоритмами.
Написать программы реализующие алгоритм [RSA](https://en.wikipedia.org/wiki/RSA_(cryptosystem)).  
>
**Генерация ключей**
+ Прочитать из консоли числа p и q.
+ Вычислить n и λ(n), где λ функция Кармайкла.
+ Сгенерировать открытую экспоненту e (1 < e < λ(n)) и проверить что она является взаимно простой с λ(n).
+ Вычислить закрытую экспоненту d, вывести открытый ключ (n, e) и закрытый ключ (n, d).
>
**Шифрование текста**
+ Прочитать из консоли открытый ключ (n, e).
+ Прочитать сообщение M (число меньшее n), зашифровать его алгоритмом RSA и вывести результат.
>
**Расшифровка текста**
+ Прочитать из консоли закрытый ключ (n, d).
+ Прочитать зашифрованное сообщение, расшифровать его алгоритмом RSA и вывести результат.
>
**(дополнительно, +2 балла)** 
+ Сделать возможность шифровать текстовые сообщения произвольной длины.
---
**3**. Цель: научиться работе с криптографическими сертификатами и изучить использование OpenSSL для их создания.
>
**Часть 1. Работа с OpenSSL**
+ Создать корневой сертификат с помощью OpenSSL (`openssl req -new -config ca.conf -x509 -out ca.crt -keyout=ca.key`), подготовив конфиг таким образом, чтобы `openssl x509 -in ca.crt -text` выдавал расшифровку вида:
```
Certificate:
    Data:
        Version: 3 (0x2)
        Serial Number: ...
    Signature Algorithm: sha256WithRSAEncryption
        Issuer: C = RU, L = Novosibirsk, O = Novosibirsk State Technical University, CN = <Some name>
        Validity
            Not Before: ...
            Not After : ...
        Subject: C = RU, L = Novosibirsk, O = Novosibirsk State Technical University, CN = <Some name>
        Subject Public Key Info:
            Public Key Algorithm: rsaEncryption
                Public-Key: (2048 bit)
                Modulus: ...
                Exponent: ...
        X509v3 extensions:
            X509v3 Key Usage: critical
                Certificate Sign
            X509v3 Basic Constraints: critical
                CA:TRUE, pathlen:1
    Signature Algorithm: sha256WithRSAEncryption
...
```
+ Создать запрос клиентского сертификата (файл `.csr`) и приватный ключ с помощью OpenSSL (`openssl req -new -config client.conf -out client.csr -keyout=client.key`), подготовив конфиг таким образом, чтобы `openssl req -in client.csr -text` выдавал расшифровку вида:
```
Certificate Request:
    Data:
        Version: 1 (0x0)
        Subject: CN = <ФИО 1, ФИО 2, ...>, C = RU, L = Novosibirsk, O = Novosibirsk State Technical University
        Subject Public Key Info:
            Public Key Algorithm: rsaEncryption
                Public-Key: (2048 bit)
                Modulus: ...
                Exponent: ...
        Attributes:
        Requested Extensions:
            X509v3 Key Usage:
                Digital Signature
            X509v3 Extended Key Usage:
                TLS Web Client Authentication
            X509v3 Basic Constraints:
                CA:FALSE
    Signature Algorithm: sha256WithRSAEncryption
...

```
+ Создать запрошенный сертификат, подписав его с помощью корневого (`openssl x509 -req -extfile client.conf -in client.csr -CA ca.crt -CAkey ca.key -CAcreateserial -out client.crt`), подготовив конфиг таким образом, чтобы `openssl x509 -in client.crt -text` выдавал расшифровку вида: 
```
Certificate:
    Data:
        Version: 3 (0x2)
        Serial Number: ...
    Signature Algorithm: sha256WithRSAEncryption
        Issuer: C = RU, L = Novosibirsk, O = Novosibirsk State Technical University, CN = <Some name>
        Validity
            Not Before: ...
            Not After : ...
        Subject: C = RU, L = Novosibirsk, O = Novosibirsk State Technical University, CN = <ФИО 1, ФИО 2, ...>
        Subject Public Key Info:
            Public Key Algorithm: rsaEncryption
                Public-Key: (2048 bit)
                Modulus: ...
                Exponent: ...
        X509v3 extensions:
            X509v3 Basic Constraints:
                CA:FALSE
            X509v3 Key Usage:
                Digital Signature
            X509v3 Extended Key Usage:
                TLS Web Client Authentication
    Signature Algorithm: sha256WithRSAEncryption
…
```
+ Все подготовленные конфиги и полученные расшифровки добавить в отчет.
>
**Часть 2. Подписать сертификат у преподавателя**
+ Загрузить файл запроса сертификата методом POST на адрес https://istupakov.ddns.net:4559/api/csr. Запомнить полученный в ответ в Location Header адрес для скачивания сертификата.
Запрос можно сделать с помощью утилиты `Curl`:\
`curl https://istupakov.ddns.net:4559/api/csr -F file=@client.csr --cacert cryptolab-ca.crt -v` \
Файл `cryptolab-ca.crt`:
```
-----BEGIN CERTIFICATE-----
MIIDijCCAnKgAwIBAgIUMA3EDa756TtXX0yCrUBwNxTfxdYwDQYJKoZIhvcNAQEL
BQAwazELMAkGA1UEBhMCUlUxFDASBgNVBAcMC05vdm9zaWJpcnNrMS8wLQYDVQQK
DCZOb3Zvc2liaXJzayBTdGF0ZSBUZWNobmljYWwgVW5pdmVyc2l0eTEVMBMGA1UE
AwwMQ3J5cHRvTGFiIENBMB4XDTIxMTAxNDE1MTE1MFoXDTIyMTAxNDE1MTE1MFow
azELMAkGA1UEBhMCUlUxFDASBgNVBAcMC05vdm9zaWJpcnNrMS8wLQYDVQQKDCZO
b3Zvc2liaXJzayBTdGF0ZSBUZWNobmljYWwgVW5pdmVyc2l0eTEVMBMGA1UEAwwM
Q3J5cHRvTGFiIENBMIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAxkFH
EMmYKuQRUH5LtM1qb0gpq2OfeJCVOZPK91oO2dOY7iKiQRmXkHSKCnT/0Gryd+Kv
Xdw/Rr7c2K9NE5oFfKA4X9ubj3LrVKF98IeqEQcWLYWgQiAtAgp//VCT5qdqTYfC
a+i/C7Nw0pwe8qmkAov3oBgmc/BwcsolgZdzevEmOZUohkW7c29I9MCVjlF9YdjR
pu291/llFKpH5hEGlPRC5P6bKYR07IlL1TGUfY0wZIXdjsHAWJfj7bCm1ucUygXL
myL8CMjInSnoQOf9McjqIKevtYhX1hqOMdSjCKf2V091FGLrlZI7IHaZ/YMxXBHR
kK9dAgvsBiFlMEdkzQIDAQABoyYwJDAOBgNVHQ8BAf8EBAMCAgQwEgYDVR0TAQH/
BAgwBgEB/wIBATANBgkqhkiG9w0BAQsFAAOCAQEAZAfjRfnbClwb/Z1RqxbWogXG
SGCiTh2pQTMFcN0dyyNKyY58Wo1NK3DKOibM8mihhQP1vKnwvG2DwiBBXxTWjjmz
CDUnEG2RKussNAPhW61V2MASqtbgkF26B562UUkqSbqbGytx3xB2e5liOtrPUBVe
ja+JnR9cZFW/BP6DM7sc9faZDunVQG7b7b8rLfiocC/1Geb3cxhvMA8/Bm6IHCLQ
eSvOZcslhUIL4G1Rz+EJcMR5aj62a1IJJFh2hGwRDK1D0kgELnhGm7nqeCtxe5Xs
NlaWjg68jZzHcdaydh0VPiiBqXkgA7c33yOWER7C6A+OTRKtWnsbRnyCf4wHFg==
-----END CERTIFICATE-----
```
+ Подойти к преподавателю и попросить подписать ваш запрос.
+ Скачать подписанный сертификат, вставить в отчет его адрес и расшифровку.
>
**Часть 3. Работа с TLS**
+ Отправить от вашей бригады некоторое сообщение в чат. Для этого необходимо отправить POST запрос на адрес https://istupakov.ddns.net:4559/api/chat/message в теле которого, будет строка с сообщением (тело запроса должно быть в формате JSON). Для аутентификации в чате необходимо использовать полученные ранее сертификаты (можно сделать с помощью утилиты Curl).
+ Привести результаты запросов с сертификатами из части 1 и из части 2 в отчете.
+ (дополнительно, +2 балла) Продемонстрировать доступ в чат из браузера на ПК.
+ (дополнительно, +3 балла) Продемонстрировать доступ в чат из браузера на телефоне.
> Данное практическое задание выполняется по мануалам. Приведены только конфигурационные файлы.

