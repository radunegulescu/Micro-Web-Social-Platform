from testFramework import add_a_friend
from testFramework import login
from testFramework import view_friend_profile
from selenium import webdriver

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

    def test_view_friend_profile_successful(self):
        self.test_login_successful()

        driver.get("http://localhost:54805/Users")
        
        user = "andreineagu672@gmail.com"
        assert(view_friend_profile.press_only_friends_button(driver))
        assert(add_a_friend.enter_user_name(driver, user))
        assert(add_a_friend.press_search_user_button(driver))
        assert(view_friend_profile.press_view_pofile_button(driver))
        assert(view_friend_profile.test_profile_name(driver, user))
        assert(driver.current_url is not None and driver.current_url ==
               "http://localhost:54805/")

    def test_view_friend_profile_failed(self):
        self.test_login_successful()

        driver.get("http://localhost:54805/Users")
        
        user = "protocaliu"
        assert(view_friend_profile.press_only_friends_button(driver))
        assert(add_a_friend.enter_user_name(driver, user))
        assert(add_a_friend.press_search_user_button(driver))
        assert(view_friend_profile.press_view_pofile_button(driver))
        assert(view_friend_profile.test_profile_name(driver, user))
        assert(driver.current_url is not None and driver.current_url ==
               "http://localhost:54805/")
    