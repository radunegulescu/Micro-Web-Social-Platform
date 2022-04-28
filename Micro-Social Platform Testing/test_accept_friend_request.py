from testFramework import login
from testFramework import logoff
from testFramework import accept_friend_request
from testFramework import add_a_friend
from selenium import webdriver

driver = webdriver.Chrome()

'''
For testing:
andreineagu672@gmail.com
Parola123!

marinela@gmail.com
username: Marinela
*123Marinela

adela@gmail.com
username: Adela
*123Adela

Logare ca andreineagu672@gmail.com -> Cauta dupa Marinela -> Send FR lui marinela@gmail.com 
-> deconectare -> Conectare ca  marinela@gmail.com  -> accept friend request 
'''

def setup_module():
    driver.get("http://localhost:54805/Account/Login")

class Test_Accept_Friend_Request():
    def teardown_method(self):
        driver.get("http://localhost:54805/Account/Login")

    def test_add_friends_successful(self):
        # add a friend from andreineagu
        assert(login.enter_email(driver, "andreineagu672@gmail.com"))
        assert(login.enter_password(driver, "Parola123!"))
        assert(login.press_login_button(driver))
        assert(driver.current_url is not None and driver.current_url ==
               "http://localhost:54805/")

        # first friend request
        driver.get("http://localhost:54805/Users")

        assert(add_a_friend.enter_user_name(driver, "Marinela"))
        assert(add_a_friend.press_search_user_button(driver))
        assert(add_a_friend.press_add_friend_button(driver))
        assert(driver.current_url is not None and driver.current_url ==
               "http://localhost:54805/Users")

        # second friend request
        driver.get("http://localhost:54805/Users")

        assert(add_a_friend.enter_user_name(driver, "Adela"))
        assert(add_a_friend.press_search_user_button(driver))
        assert(add_a_friend.press_add_friend_button(driver))
        assert(driver.current_url is not None and driver.current_url ==
               "http://localhost:54805/Users")

    def test_add_friend_failed(self):
        '''
        A user has already sent friend request to user scenario
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


    def test_accept_friend_request_successful(self):
        '''
        Marinela accepts friend request
        '''
        #logoff
        assert(logoff.press_logoff_button(driver))
        assert(driver.current_url is not None and driver.current_url ==
               "http://localhost:54805/Account/Login?ReturnUrl=%2F")

        # login as Marinela
        driver.get("http://localhost:54805/Account/Login") #resolving some bug
        assert(login.enter_email(driver, "marinela@gmail.com"))
        assert(login.enter_password(driver, "*123Marinela"))
        assert(login.press_login_button(driver))
        assert(driver.current_url is not None and driver.current_url ==
               "http://localhost:54805/")
        driver.get("http://localhost:54805/Users/Requests")

        assert(accept_friend_request.press_accept_button(driver))
        assert(driver.current_url is not None and driver.current_url.startswith(
            'http://localhost:54805/Users'))


    def test_reject_friend_request_successful(self):
        '''
        Adela rejects friend request
        '''
        #logoff
        assert(logoff.press_logoff_button(driver))
        assert(driver.current_url is not None and driver.current_url ==
               "http://localhost:54805/Account/Login?ReturnUrl=%2F")
               
        # login as Adela
        driver.get("http://localhost:54805/Account/Login") #resolving some bug
        assert(login.enter_email(driver, "adela@gmail.com"))
        assert(login.enter_password(driver, "*123Adela"))
        assert(login.press_login_button(driver))
        assert(driver.current_url is not None and driver.current_url ==
               "http://localhost:54805/")
        driver.get("http://localhost:54805/Users/Requests")

        assert(accept_friend_request.press_reject_button(driver))
        assert(driver.current_url is not None and driver.current_url.startswith(
            'http://localhost:54805/Users'))


    def test_accept_friend_request_failed(self):
        '''
        There is no friend request scenario
        '''
        #logoff
        assert(logoff.press_logoff_button(driver))
        assert(driver.current_url is not None and driver.current_url ==
               "http://localhost:54805/Account/Login?ReturnUrl=%2F")
               
        # login as andreineagu
        driver.get("http://localhost:54805/Account/Login") #resolving some bug
        assert(login.enter_email(driver, "andreineagu672@gmail.com"))
        assert(login.enter_password(driver, "Parola123!"))
        assert(login.press_login_button(driver))
        assert(driver.current_url is not None and driver.current_url ==
               "http://localhost:54805/")
        driver.get("http://localhost:54805/Users/Requests")

        assert(accept_friend_request.press_accept_button(driver) == False)
       