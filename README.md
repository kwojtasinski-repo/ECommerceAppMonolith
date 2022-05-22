# ECommerceAppMonolith
# Project
This project is based on existing solution ECommerceApp. For a better scalability and adding new features, it was decided to divide ECommerceApp into 5 modules: Contacts, Currencies, Items, Sales, Users. 
Every module has its own scheme in database. 
- Contacts module has information about customer. 
- Currencies cyclically downloads data from API NBP using custom background job. Every new rate publish new event.
- Items module stores data about items in ECommerceApp
- Sales based on the data of module Item shows which products is ordered or is in ItemCart. Additionally after add item to ItemCart the copy of Item is created as a snapshot in db. Module also responds to Currencies events especially on new currency rates events and subcribes one events to items module events like create, update item to sale. This provides the sales module to have the items that have been approved in the Items module, locally at in the own schema
- Users stores information about permissions, role of users

AppInitlizer is responsible for migration data. Its implementation is located in folder 'src/Shared/ECommerce.Shared.Infrastructure'. This project has unit and integration tests written using xUnit. Every module has example scenario in rest file like for example Currencies module in the file 'Currencies.rest'. 
Due to the future changes to the current architecture into microservices, decided to add Gateway to this project.

#Technologies:
- .Net 6
- Postgres
- React
- Yarp
- Axios
- Humanizer
- Flurl
- Scrutor
- JWT
- Cronos
- Jest

#Database
Below shown db schemas of modules:

- Contacts 

![](https://raw.githubusercontent.com/kamasjdev/ECommerceAppMonolith/main/images/contacts_diagram.png)

- Currencies

![](https://raw.githubusercontent.com/kamasjdev/ECommerceAppMonolith/main/images/currencies_diagram.png)

- Items

![](https://raw.githubusercontent.com/kamasjdev/ECommerceAppMonolith/main/images/items_diagram.png)

- Sales

![](https://raw.githubusercontent.com/kamasjdev/ECommerceAppMonolith/main/images/sales_diagram.png)

- Users

![](https://raw.githubusercontent.com/kamasjdev/ECommerceAppMonolith/main/images/users_diagram.png)

#Screens

![](https://raw.githubusercontent.com/kamasjdev/ECommerceAppMonolith/main/images/image_1.png)
![](https://raw.githubusercontent.com/kamasjdev/ECommerceAppMonolith/main/images/image_2.png)
![](https://raw.githubusercontent.com/kamasjdev/ECommerceAppMonolith/main/images/image_3.png)
![](https://raw.githubusercontent.com/kamasjdev/ECommerceAppMonolith/main/images/image_4.png)
![](https://raw.githubusercontent.com/kamasjdev/ECommerceAppMonolith/main/images/image_5.png)
![](https://raw.githubusercontent.com/kamasjdev/ECommerceAppMonolith/main/images/image_6.png)
![](https://raw.githubusercontent.com/kamasjdev/ECommerceAppMonolith/main/images/image_7.png)
![](https://raw.githubusercontent.com/kamasjdev/ECommerceAppMonolith/main/images/image_8.png)
![](https://raw.githubusercontent.com/kamasjdev/ECommerceAppMonolith/main/images/image_9.png)
![](https://raw.githubusercontent.com/kamasjdev/ECommerceAppMonolith/main/images/image_10.png)
![](https://raw.githubusercontent.com/kamasjdev/ECommerceAppMonolith/main/images/image_11.png)
![](https://raw.githubusercontent.com/kamasjdev/ECommerceAppMonolith/main/images/image_12.png)
![](https://raw.githubusercontent.com/kamasjdev/ECommerceAppMonolith/main/images/image_13.png)
![](https://raw.githubusercontent.com/kamasjdev/ECommerceAppMonolith/main/images/image_14.png)
![](https://raw.githubusercontent.com/kamasjdev/ECommerceAppMonolith/main/images/image_15.png)
![](https://raw.githubusercontent.com/kamasjdev/ECommerceAppMonolith/main/images/image_16.png)
![](https://raw.githubusercontent.com/kamasjdev/ECommerceAppMonolith/main/images/image_17.png)
![](https://raw.githubusercontent.com/kamasjdev/ECommerceAppMonolith/main/images/image_18.png)
![](https://raw.githubusercontent.com/kamasjdev/ECommerceAppMonolith/main/images/image_19.png)
![](https://raw.githubusercontent.com/kamasjdev/ECommerceAppMonolith/main/images/image_20.png)
![](https://raw.githubusercontent.com/kamasjdev/ECommerceAppMonolith/main/images/image_21.png)
![](https://raw.githubusercontent.com/kamasjdev/ECommerceAppMonolith/main/images/image_22.png)
![](https://raw.githubusercontent.com/kamasjdev/ECommerceAppMonolith/main/images/image_23.png)
![](https://raw.githubusercontent.com/kamasjdev/ECommerceAppMonolith/main/images/image_24.png)