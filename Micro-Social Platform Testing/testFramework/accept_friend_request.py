from selenium.common.exceptions import *
from selenium.webdriver.support import expected_conditions as EC
from selenium import webdriver
from selenium.webdriver.common.by import By
from selenium.webdriver.support.wait import WebDriverWait
import logging

logger = logging.getLogger(__name__)

def press_accept_button(driver):
    try:
        login_button = WebDriverWait(driver, 10).until(
            EC.visibility_of_element_located((By.XPATH, "/html/body/div[2]/div/div/form[1]/button")))
        login_button.click()
        return True
    except:
        logger.warning("Couldn't press the accept button")
        return False

def press_reject_button(driver):
    try:
        login_button = WebDriverWait(driver, 10).until(
            EC.visibility_of_element_located((By.XPATH, "/html/body/div[2]/div/div/form[2]/button")))
        login_button.click()
        return True
    except:
        logger.warning("Couldn't press the reject button")
        return False