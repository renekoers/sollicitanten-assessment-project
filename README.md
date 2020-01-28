Project Sylveon

Installation guide:
Please follow the following steps in the installation guide:
- Install MySQL.
- Connect to MySQL.
- Run the following commands:

- CREATE USER 'init'@'localhost' IDENTIFIED BY 'password';
- GRANT CREATE USER ON *.* TO 'init'@'localhost';
- GRANT CREATE ON *.* TO 'init'@'localhost';
- GRANT GRANT OPTION ON *.* TO 'init'@'localhost';

If the user 'init' already exists, then you have to change the password if 'init' in the file <ENTER_FILENAME>.
