from testFramework import add_post, groups
from testFramework import login
from testFramework import view_friend_profile
from selenium import webdriver

driver = webdriver.Chrome()

def setup_module():
    driver.maximize_window()
    driver.get("http://localhost:54805/Account/Login")

class Test_Create_Post():
    def teardown_method(self):
        pass
        # driver.get("http://localhost:54805/Account/Login")

    def test_login_successful(self):
        assert(login.enter_email(driver, "user@gmail.com"))
        assert(login.enter_password(driver, "!Username06"))
        assert(login.press_login_button(driver))
        assert(driver.current_url is not None and driver.current_url ==
               "http://localhost:54805/")

    def test_create_post(self):
        driver.get("http://localhost:54805")
        assert(add_post.press_add_post(driver))

        assert(add_post.enter_title_post(driver,"Something new"))
        assert(add_post.enter_content_post(driver, "Something to test"))
        assert(add_post.press_submit_post(driver))



    # def test_add_comment(self):
    #     # prepare for adding the comment
    #     self.test_login_successful()
    #     #assert (add_post.press_show_more(driver))
    #     #assert(add_post.enter_comm_content(driver,"New Comm"))
    #     #assert(add_post.press_add_comment(driver))
    #     #assert (driver.current_url is not None and driver.current_url ==
    #             #"http://localhost:54805/")

