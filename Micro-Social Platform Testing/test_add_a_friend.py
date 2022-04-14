from testFramework import add_a_friend
from testFramework import login
from selenium import webdriver

'''
 User account for testing:
 marinela@gmail.com
 username: Marinela
 *123Marinela
'''
driver = webdriver.Chrome()

def setup_module():
    driver.get("http://localhost:54805/Account/Login")

class Test_Add_Friend():
    def teardown_method(self):
        driver.get("http://localhost:54805/Account/Login")

    def test_add_friend_successful(self):
        # login

        assert(login.enter_email(driver, "andreineagu672@gmail.com"))
        assert(login.enter_password(driver, "Parola123!"))
        assert(login.press_login_button(driver))
        assert(driver.current_url is not None and driver.current_url ==
               "http://localhost:54805/")

        driver.get("http://localhost:54805/Users")

        assert(add_a_friend.enter_user_name(driver, "Marinela"))
        assert(add_a_friend.press_search_user_button(driver))
        assert(add_a_friend.press_add_friend_button(driver))
        assert(driver.current_url is not None and driver.current_url ==
               "http://localhost:54805/Users")
    
    
    def test_add_friend_failed(self):
        '''
        A user is already friend with a user scenario
        '''
         # login

        assert(login.enter_email(driver, "andreineagu672@gmail.com"))
        assert(login.enter_password(driver, "Parola123!"))
        assert(login.press_login_button(driver))
        assert(driver.current_url is not None and driver.current_url ==
               "http://localhost:54805/")

        driver.get("http://localhost:54805/Users")

        assert(add_a_friend.enter_user_name(driver, "Marinela"))
        assert(add_a_friend.press_search_user_button(driver))
        assert(add_a_friend.press_add_friend_button(driver) == False)

