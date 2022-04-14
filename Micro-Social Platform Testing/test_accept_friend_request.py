from testFramework import login
from testFramework import logoff
from testFramework import accept_friend_request
from testFramework import add_a_friend
from selenium import webdriver

driver = webdriver.Chrome()

'''
For testing:
andreineagu672@gmail.com
Parola1234!

marinela@gmail.com
username: Marinela
*123Marinela

Logare ca andreineagu672@gmail.com -> Cauta dupa Marinela -> Send FR lui marinela@gmail.com 
-> deconectare -> Conectare ca  marinela@gmail.com  -> accept friend request 
'''
def setup_module():
    driver.get("http://localhost:54805/Account/Login")

class Test_Accept_Friend_Request():
    def teardown_method(self):
        driver.get("http://localhost:54805/Account/Login")

    def test_login_successful(self, email, password):
        assert(login.enter_email(driver, email))
        assert(login.enter_password(driver, password))
        assert(login.press_login_button(driver))
        assert(driver.current_url is not None and driver.current_url ==
               "http://localhost:54805/")

    def test_add_friend_successful(self):
        # add a friend from andreineagu
        self.test_login_successful("andreineagu672@gmail.com", "Parola1234!")

        driver.get("http://localhost:54805/Users")

        assert(add_a_friend.enter_user_name(driver, "Marinela"))
        assert(add_a_friend.press_search_user_button(driver))
        assert(add_a_friend.press_add_friend_button(driver))
        assert(driver.current_url is not None and driver.current_url ==
               "http://localhost:54805/Users")
    
    def test_add_friend_failed(self):
        # add a friend from andreineagu
        self.test_login_successful("andreineagu672@gmail.com", "Parola1234!")

        driver.get("http://localhost:54805/Users")

        assert(add_a_friend.enter_user_name(driver, "Catalina"))
        assert(add_a_friend.press_search_user_button(driver))
        assert(add_a_friend.press_add_friend_button(driver))
        assert(driver.current_url is not None and driver.current_url ==
               "http://localhost:54805/Users")

    def test_logoff_successful(self):
        # add a friend from andreineagu
       
        assert(logoff.press_logoff_button(driver))
        assert(driver.current_url is not None and driver.current_url ==
               "http://localhost:54805/Account/Login?ReturnUrl=%2F")

    def test_accept_friend_request_successful(self):
        self.test_add_friend_successful()

        # logoff
        self.test_logoff_successful()

        # login as marinela
        driver.get("http://localhost:54805/Account/Login") #resolving some bug
        self.test_login_successful("marinela@gmail.com", "*123Marinela")
        driver.get("http://localhost:54805/Users/Requests")

        assert(accept_friend_request.press_accept_button(driver))
        assert(driver.current_url is not None and driver.current_url.startswith(
            'http://localhost:54805/Users'))

    def test_reject_friend_request_successful(self):
        self.test_add_friend_successful()

        # logoff
        self.test_logoff_successful()

        # login as marinela
        driver.get("http://localhost:54805/Account/Login") #resolving some bug
        self.test_login_successful("marinela@gmail.com", "*123Marinela")
        driver.get("http://localhost:54805/Users/Requests")

        assert(accept_friend_request.press_reject_button(driver))
        assert(driver.current_url is not None and driver.current_url.startswith(
            'http://localhost:54805/Users'))


    def test_accept_friend_request_failed(self):
        self.test_add_friend_failed()

        # logoff
        self.test_logoff_successful()

        # login as marinela
        driver.get("http://localhost:54805/Account/Login") #resolving some bug
        self.test_login_successful("marinela@gmail.com", "*123Marinela")
        driver.get("http://localhost:54805/Users/Requests")

        assert(accept_friend_request.press_reject_button(driver))
        assert(driver.current_url is not None and driver.current_url.startswith(
            'http://localhost:54805/Users'))
