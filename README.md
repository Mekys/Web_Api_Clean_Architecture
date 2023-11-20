## Web_Api_Clean_Architecture
# Подготовка
Перед использованием добавить в файл /Infrastructure/AppSettings.json вашу строку подключения к базе данных PostgreSQL, где созданы три сущности.
1) User(UserId (int), Login (text), Password (text), CreatedDate (Date), UserGroupId (int), UserStateId (int))
2) UserGroup(UserGroupId (int), Code (int: 0 - User, 1 - Admin), Description (text))
3) UserState(UserStateId (int), Code (int: 0 - Blocked, 1 - Active), Description (text))
# Детали
Запросы на удаление и добавление новых аккаунтов доступны только пользователям, которые авторизовались с правами аддминистратора (UserGroup.Code = 1). Также система не позволяет добавлять нового пользователя с правами алминистратора или удалять существующего. Запросы на получение данных доступны всем пользователям. 
