<img width="1420" height="856" alt="image" src="https://github.com/user-attachments/assets/71dea6ce-0686-48e0-b7a0-7a5c96b59973" />
**@startuml
left to right direction

actor Resident
actor Admin
actor Worker
actor Manager

usecase "Подать заявку" as UC1
usecase "Просмотреть статус" as UC2
usecase "Обработать заявку" as UC3
usecase "Назначить исполнителя" as UC4
usecase "Просмотреть историю" as UC5

Resident --> UC1
Resident --> UC2

Admin --> UC3
Admin --> UC4
Admin --> UC5

Worker --> UC3 : <<выполняет>>

Manager --> UC5

UC4 --> UC3 : <<добавляет>>

@enduml**
