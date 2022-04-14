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

    def test_login_successful(self):
        assert(login.enter_email(driver, "marinela@gmail.com"))
        assert(login.enter_password(driver, "*123Marinela"))
        assert(login.press_login_button(driver))
        assert(driver.current_url is not None and driver.current_url ==
               "http://localhost:54805/")

    def test_add_friend_successful(self):
        self.test_login_successful()

        driver.get("http://localhost:54805/Users")

        assert(add_a_friend.enter_user_name(driver, "Marinela"))
        assert(add_a_friend.press_search_user_button(driver))
        assert(add_a_friend.press_add_friend_button(driver))
        assert(driver.current_url is not None and driver.current_url ==
               "http://localhost:54805/Users")
    
    
    def test_add_friend_failed(self):
        self.test_login_successful()

        driver.get("http://localhost:54805/Users")

        assert(add_a_friend.enter_user_name(driver, "Lucia"))
        assert(add_a_friend.press_search_user_button(driver))
        assert(add_a_friend.press_add_friend_button(driver))
        assert(driver.current_url is not None and driver.current_url ==
               "http://localhost:54805/Users")
