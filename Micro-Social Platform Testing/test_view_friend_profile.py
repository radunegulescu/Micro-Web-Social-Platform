from testFramework import add_a_friend
from testFramework import login
from testFramework import view_friend_profile
from selenium import webdriver

driver = webdriver.Chrome()

def setup_module():
    driver.get("http://localhost:54805/Account/Login")

class Test_View_Friend():
    def teardown_method(self):
        driver.get("http://localhost:54805/Account/Login")

    def test_view_friend_profile_successful(self):
        # login
        assert(login.enter_email(driver, "andreineagu672@gmail.com"))
        assert(login.enter_password(driver, "Parola123!"))
        assert(login.press_login_button(driver))
        assert(driver.current_url is not None and driver.current_url ==
               "http://localhost:54805/")

        driver.get("http://localhost:54805/Users")
        
        user = "Marinela"
        assert(view_friend_profile.press_only_friends_button(driver))
        assert(add_a_friend.enter_user_name(driver, user))
        assert(add_a_friend.press_search_user_button(driver))
        assert(view_friend_profile.press_view_pofile_button(driver))
        assert(view_friend_profile.test_profile_name(driver, user))
        assert((driver.current_url is not None) 
                        and 
               ("http://localhost:54805/Users/Show/" in  driver.current_url)
              )

    def test_view_friend_profile_failed(self):
        "Empty searchg scenario"
        
        #login
        assert(login.enter_email(driver, "andreineagu672@gmail.com"))
        assert(login.enter_password(driver, "Parola123!"))
        assert(login.press_login_button(driver))
        assert(driver.current_url is not None and driver.current_url ==
               "http://localhost:54805/")

        driver.get("http://localhost:54805/Users")
        
        user = "marinela@gmail.com" # searching by username, not by email
        # empty search 
        assert(view_friend_profile.press_only_friends_button(driver))
        assert(add_a_friend.enter_user_name(driver, user))
        assert(add_a_friend.press_search_user_button(driver))
        assert((driver.current_url is not None) 
                        and 
               ("http://localhost:54805/Users?search=" in  driver.current_url)
              )
    