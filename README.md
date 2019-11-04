# Finances-XAF

Sealveru - Finances allows people who do not have advanced knowledge in accounting to take better control of their personal finances. It is built in 3 main blocks:

- **Core**: Provides the necessary functionality to keep a formal accounting. It contains elements such as: chart of accounts, accounting periods, closing entries, journal and more.
- **Templates**: Provides a mechanism to automate the creation of repetitive accounting entries. A base template is defined, so the user must only provide the final values ​​to apply the entry.
- **Expenses and incomes**: Abstract the process of creating accounting entries, so the end user does not need to know the internal processes, but only provide a description and an amount.

As data is entered into the system, it will provide real-time information on the individual's financial status, allowing to: search and filter existing data, extract information using pre-configured graphs and reports, and create custom reports.


> The records provided by the user are NOT stored on remote servers, but when configuring the project a storage mechanism must be defined, this can be LocalStorage or Postgresql, in any case the data remains under the control of the final user.


This is a native Windows application developed in C #, however there are other versions available:

- Django Rest Framework API
- Angular


## Tools

In order to build, test and run this solution it is necesary to install globally the following tools.

- [Visual Studio 2019](https://visualstudio.microsoft.com/es/vs/): The Visual Studio integrated development environment is a creative launching pad that you can use to edit, debug, and build code, and then publish an app.
- [XAF](https://www.devexpress.com/products/net/application_framework/): eXpressApp Framework (XAF)﻿ is a versatile application framework that allows developers to build business applications that target Windows, Web, and Mobile.
- [Python 3.x](https://www.python.org/): Python is a programming language that lets you work quickly and integrate systems more effectively.
- [SandCastle](https://github.com/EWSoftware/SHFB): The Sandcastle tools are used to create help files for managed class libraries containing both conceptual and API reference topics. 
- [GIT](https://git-scm.com/): Git is a free and open source distributed version control system designed to handle everything from small to very large projects with speed and efficiency.
- [Postgresql](https://www.postgresql.org/): (optional) PostgreSQL is a powerful, open source object-relational database system with over 30 years of active development.


## Installation

- Clone this repository.
- Install python-dotenv: ```pip install python-dotenv```.
- Fill the ```.env``` file.
  - ```EASY_TEST_EXECUTOR_PATH```: the path to TestExecutor.v19.1.exe file. *(If you did a default installation of XAF this value is already setted)*.
  - ```VISUAL_STUDIO_PATH```: the path to Visual Studio installation. *(If you did a default installation of VS this value is already setted)*.
  - ```STORAGE```: the kind of storage to use. Accepted values: ```local```, ```postgresql```. 


**Build with VS**
 
- Open the ```src\Sealveru.Finances.sln``` file.
- Click on __Start__ button in VS.

**Build without VS**

- Run the ```build.cmd``` script as administrator. This script will build the whole solution using **MSBuild**, run unit tests using **MSTest**, run functional tests using **EasyTest** and copy the final binaries to ```artifacts``` folder. To avoid to run tests, the ```--no-tests``` flag can be passed to the script.
- You can run the ```artifacts\Sealveru.Finances.exe``` file to use the aplicacion.


## Usage

TODO: Add a better explanation and screenshots about how to use the application.

**Default credentials**

Use the following credential to login at first time. It is possible to change the password or create new users in the admin section.
- *Username*: admin
- *Password*: admin

## License

[GNU GPL v3](https://www.gnu.org/licenses/gpl-3.0.html)