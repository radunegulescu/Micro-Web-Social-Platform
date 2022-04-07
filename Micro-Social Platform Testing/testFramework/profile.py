from selenium.common.exceptions import *
from selenium.webdriver.support import expected_conditions as EC
from selenium.webdriver.common.by import By
from selenium.webdriver.support.wait import WebDriverWait
import logging

logger = logging.getLogger(__name__)


def click_create(driver):
    try:
        create_button = WebDriverWait(driver, 10).until(
            EC.visibility_of_element_located((By.XPATH, "/html/body/div[1]/div/div[2]/ul/li[6]/a")))
        create_button.click()
        return True
    except NoSuchElementException:
        logger.warning("Clicking create failed")
        return False


def click_myprofile(driver):
    try:
        # profile_button = driver.find_elements_by_xpath("//*[contains(text(), 'My Profile')]")
        profile_button = WebDriverWait(driver, 10).until(
            EC.visibility_of_element_located((By.XPATH, "/html/body/div[1]/div/div[2]/ul/li[6]/a")))
        if profile_button.text != "My Profile":
            return False
        profile_button.click()
        return True
    except NoSuchElementException:
        logger.warning("Clicking My Profile failed")
        return False


def check_myprofile(driver):
    try:
        # profile_button = driver.find_elements_by_xpath("//*[contains(text(), 'My Profile')]")
        profile_button = WebDriverWait(driver, 10).until(
            EC.visibility_of_element_located((By.XPATH, "/html/body/div[1]/div/div[2]/ul/li[6]/a")))
        if profile_button.text != "My Profile":
            return False
        return True
    except NoSuchElementException:
        logger.warning("My Profile button failed")
        return False


def enter_name(driver, name):
    try:
        name_field = WebDriverWait(driver, 10).until(
            EC.visibility_of_element_located((By.XPATH, "//*[@id='Name']")))
        name_field.send_keys(name)
        return True
    except NoSuchElementException:
        logger.warning("Entering name failed")
        return False


def enter_description(driver, description):
    try:
        name_field = WebDriverWait(driver, 10).until(
            EC.visibility_of_element_located((By.XPATH, "//*[@id='Description']")))
        name_field.send_keys(description)
        return True
    except NoSuchElementException:
        logger.warning("Entering description failed")
        return False


def select_public(driver):
    try:
        public_button = WebDriverWait(driver, 10).until(
            EC.visibility_of_element_located((By.XPATH, "//*[@id='public']")))
        public_button.click()
        return True
    except NoSuchElementException:
        logger.warning("Clicking public failed")
        return False


def select_private(driver):
    try:
        private_button = WebDriverWait(driver, 10).until(
            EC.visibility_of_element_located((By.XPATH, "//*[@id='private']")))
        private_button.click()
        return True
    except NoSuchElementException:
        logger.warning("Clicking private failed")
        return False


def select_photo(driver, path):
    try:
        file_field = WebDriverWait(driver, 10).until(
            EC.visibility_of_element_located((By.XPATH, "/html/body/div[2]/div/form/input[4]")))
        file_field.send_keys(path)
        return True
    except NoSuchElementException:
        logger.warning("Choosing photo failed")
        return False


def click_submit(driver):
    try:
        submit_button = WebDriverWait(driver, 10).until(
            EC.visibility_of_element_located((By.XPATH, "/html/body/div[2]/div/form/button")))
        submit_button.click()
        return True
    except:
        logger.warning("Couldn't press the submit button")
        return False


def delete_profile(driver):
    try:
        delete_button = WebDriverWait(driver, 10).until(
            EC.visibility_of_element_located((By.XPATH, "/html/body/div[2]/div/div[4]/div[2]/button")))
        delete_button.click()
        delete_profile_button = WebDriverWait(driver, 10).until(
            EC.visibility_of_element_located((By.XPATH, "/html/body/div[2]/div/div[4]/div[2]/div/form[2]/button")))
        delete_profile_button.click()
        return True
    except:
        logger.warning("Couldn't press the delete profile button")
        return False


def click_delete_photo(driver):
    try:
        delete_button = WebDriverWait(driver, 10).until(
            EC.visibility_of_element_located((By.XPATH, "/html/body/div[2]/div/div[4]/div[2]/button")))
        delete_button.click()
        delete_photo_button = WebDriverWait(driver, 10).until(
            EC.visibility_of_element_located((By.XPATH, "/html/body/div[2]/div/div[4]/div[2]/div/form[1]/button")))
        delete_photo_button.click()
        return True
    except:
        logger.warning("Couldn't press the delete profile photo button")
        return False


def check_no_photo(driver):
    try:
        photo = WebDriverWait(driver, 10).until(
            EC.visibility_of_element_located((By.XPATH, "/html/body/div[2]/div/div[2]/a/img")))
        return photo.get_attribute("src") == "https://img2.thejournal.ie/inline/1881369/original/?width=630&version=1881369"
    except:
        logger.warning("Couldn't find the photo")
        return False


def click_edit_profile_photo(driver):
    try:
        edit_button = WebDriverWait(driver, 10).until(
            EC.visibility_of_element_located((By.XPATH, "/html/body/div[2]/div/div[4]/div[1]/button")))
        edit_button.click()
        edit_photo_button = WebDriverWait(driver, 10).until(
            EC.visibility_of_element_located((By.XPATH, "/html/body/div[2]/div/div[4]/div[1]/div/a[2]")))
        edit_photo_button.click()
        return True
    except:
        logger.warning("Couldn't press the edit profile photo button")
        return False


def select_photo_edit(driver, path):
    try:
        file_field = WebDriverWait(driver, 10).until(
            EC.visibility_of_element_located((By.XPATH, "/html/body/div[2]/div/form/input")))
        file_field.send_keys(path)
        return True
    except NoSuchElementException:
        logger.warning("Choosing photo failed")
        return False


def click_submit_edit_photo(driver):
    try:
        submit_button = WebDriverWait(driver, 10).until(
            EC.visibility_of_element_located((By.XPATH, "/html/body/div[2]/div/form/button")))
        submit_button.click()
        return True
    except:
        logger.warning("Couldn't press the submit button")
        return False


def click_edit_profile(driver):
    try:
        edit_button = WebDriverWait(driver, 10).until(
            EC.visibility_of_element_located((By.XPATH, "/html/body/div[2]/div/div[4]/div[1]/button")))
        edit_button.click()
        edit_profile_button = WebDriverWait(driver, 10).until(
            EC.visibility_of_element_located((By.XPATH, "/html/body/div[2]/div/div[4]/div[1]/div/a[1]")))
        edit_profile_button.click()
        return True
    except:
        logger.warning("Couldn't press the edit profile button")
        return False


