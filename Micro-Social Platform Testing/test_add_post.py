from testFramework import add_post
from testFramework import login
from selenium import webdriver
import time

driver = webdriver.Chrome()

def setup_module():
    driver.maximize_window()
    driver.get("http://localhost:54805/Account/Login")

class Test_Create_Post():
    def teardown_method(self):
        driver.get("http://localhost:54805/Account/Login")

    def test_login_successful(self):
        assert(login.enter_email(driver, "user@gmail.com"))
        assert(login.enter_password(driver, "!Username06"))
        assert(login.press_login_button(driver))

    def test_create_post(self):
        self.test_login_successful()
        driver.get("http://localhost:54805")
        assert(add_post.press_add_post(driver))
        assert(add_post.enter_title_post(driver,"Something new"))
        assert(add_post.enter_content_post(driver, "Something to test"))
        assert(add_post.press_submit_post(driver))
        time.sleep(3)


    def test_edit_post(self):
        self.test_login_successful()
        driver.get("http://localhost:54805")
        assert (add_post.press_show_more(driver))
        assert(add_post.press_editpost_button(driver))
        assert(add_post.enter_empty_posttitle(driver))
        assert(add_post.enter_empty_postcontent(driver))
        assert(add_post.enter_title_post(driver,"edited title"))
        assert (add_post.enter_content_post(driver,"edited post"))
        assert (add_post.press_submit_post(driver))
        time.sleep(3)

    def test_create_comm(self):
        self.test_login_successful()
        driver.get("http://localhost:54805")
        assert (add_post.press_show_more(driver))
        assert (add_post.enter_comm_content(driver,"New comm"))
        assert(add_post.press_add_comment(driver))


    def test_edit_comm(self):
        self.test_login_successful()
        driver.get("http://localhost:54805")
        assert (add_post.press_show_more(driver))
        assert (add_post.press_edit_comm(driver))
        time.sleep(2)
        assert (add_post.enter_empty_commcontent(driver, "Edited comm"))
        assert (add_post.press_submit_edit(driver))
'''
    def test_delete_comm(self):
        self.test_login_successful()
        driver.get("http://localhost:54805")
        assert (add_post.press_show_more(driver))
        assert (add_post.press_show_more(driver))
        assert (add_post.press_delete_comment(driver))
        assert (add_post.press_show_more(driver))
        time.sleep(3)

    def test_delete_post(self):
        self.test_login_successful()
        driver.get("http://localhost:54805")
        assert(add_post.press_show_more(driver))
        time.sleep(3)
        assert(add_post.delete_post(driver))
'''







