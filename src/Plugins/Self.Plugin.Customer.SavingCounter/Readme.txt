1. admin UI to configure following values
  - initial base amount
  - delta amount max
  - counter refresh frequency in minutes
2. schedule task (running every 30 minutes) to increase the base amount by a delta amount
3. event consumer to calculate actual order total savings whenever an order is placed
4. widget plugin with a ViewComponent to render saving counter on the public site
5. javascript ajax call to get the total savings from server side
6. total savings = base amount + actual order total savings

