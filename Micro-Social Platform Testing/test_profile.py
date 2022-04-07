import os

from selenium.webdriver.common.by import By
from selenium.webdriver.support import expected_conditions as EC
from selenium.webdriver.support.wait import WebDriverWait

from testFramework import profile, login
from selenium import webdriver

driver = webdriver.Chrome()
ROOT_DIR = os.path.dirname(os.path.abspath(__file__))


def setup_module():
    driver.get("http://localhost:54805/Account/Login")
    login.enter_email(driver, "andreineagu672@gmail.com")
    login.enter_password(driver, "Parola123!")
    login.press_login_button(driver)


class Test_Profile():
    def teardown_method(self):
        pass

    def test_delete1(self):
        if profile.check_myprofile(driver):
            assert (profile.click_myprofile(driver))
            assert (profile.delete_profile(driver))

    def test_create_failed_noname(self):
        assert (profile.click_create(driver))
        assert (profile.enter_name(driver, ""))
        assert (profile.enter_description(driver, "Sunt haios!"))
        assert (profile.select_private(driver))
        assert (profile.select_photo(driver,
                                     os.path.join(os.path.sep, ROOT_DIR, "test_photos\\image-analysis.png")))
        assert (profile.click_submit(driver))
        assert (not profile.check_myprofile(driver))

    def test_create_successful_nophoto_public(self):
        assert (profile.click_create(driver))
        assert (profile.enter_name(driver, "Radu"))
        assert (profile.enter_description(driver, "Sunt haios!"))
        assert (profile.select_public(driver))
        assert (profile.click_submit(driver))
        assert (profile.check_myprofile(driver))

    def test_delete2(self):
        assert (profile.delete_profile(driver))
        assert (not profile.check_myprofile(driver))

    def test_create_successful_private(self):
        assert (profile.click_create(driver))
        assert (profile.enter_name(driver, "Radu"))
        assert (profile.enter_description(driver, "Sunt haios!"))
        assert (profile.select_private(driver))
        assert (profile.select_photo(driver,
                                     os.path.join(os.path.sep, ROOT_DIR, "test_photos\\image-analysis.png")))
        assert (profile.click_submit(driver))
        assert (profile.check_myprofile(driver))

    def test_delete_photo(self):
        assert (profile.click_delete_photo(driver))
        assert (profile.check_no_photo(driver))

    def test_edit_profile_photo(self):
        assert (profile.click_edit_profile_photo(driver))
        assert (profile.select_photo_edit(driver,
                                          os.path.join(os.path.sep, ROOT_DIR, "test_photos\\image-analysis.png")))
        assert (profile.click_submit_edit_photo(driver))
        assert (not profile.check_no_photo(driver))

    def test_edit_successful(self):
        assert (profile.click_edit_profile(driver))
        assert (profile.enter_name(driver, "Radu"))
        assert (profile.enter_description(driver, "Sunt haios!"))
        assert (profile.select_public(driver))
        assert (profile.click_submit(driver))
        assert (profile.check_myprofile(driver))
