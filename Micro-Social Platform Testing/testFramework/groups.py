from selenium.common.exceptions import *
from selenium.webdriver.support import expected_conditions as EC
from selenium import webdriver
from selenium.webdriver.common.by import By
from selenium.webdriver.support.wait import WebDriverWait
import logging

logger = logging.getLogger(__name__)


def get_all_groups(driver):
    try:
        groups = []
        all_groups_widget = driver.find_elements(
            By.CLASS_NAME, "panel-default")
        for group in all_groups_widget:
            groupName = group.find_element(By.CLASS_NAME, "post-title").text
            groupButton = group.find_element(
                By.CLASS_NAME, "btn-sm")
            groupData = {
                'Name': groupName,
                'Submit': groupButton
            }
            groups.append(groupData)
        return groups
    except:
        logger.warning("Couldn't get group data")
        raise


def join_group(driver, groupName):
    try:
        all_groups = get_all_groups(driver)
        for group in all_groups:
            logger.warning(group['Name'])
            if group['Name'] == groupName:
                group['Submit'].click()
                return True
        logger.error("Couldn't find the named group in list")
        return False
    except:
        logger.warning("Couldn't join the group")
        return False


def show_group(driver, groupName):
    try:
        all_groups = get_all_groups(driver)
        for group in all_groups:
            if group['Name'] == groupName:
                group['Submit'].click()
                return True
        logger.error("Couldn't find the named group in list")
        return False
    except:
        logger.warning("Couldn't join the group")
        return False


def create_new_group(driver, groupName, groupDescription):
    try:
        plus_widget = driver.find_element(
            By.XPATH, "/html/body/div[2]/div[1]/div/a")
        plus_widget.click()
        group_name_input = driver.find_element(By.XPATH, "//*[@id='Name']")
        group_name_input.send_keys(groupName)
        group_desc_input = driver.find_element(
            By.XPATH, "//*[@id='Description']")
        group_desc_input.send_keys(groupDescription)
        add_button = driver.find_element(
            By.XPATH, "/html/body/div[2]/div/form/button")
        add_button.click()
        return True
    except:
        logger.warning("Couldn't add a new group")
        return False


def find_group(driver, groupName):
    try:
        all_groups = get_all_groups(driver)
        for group in all_groups:
            if group['Name'] == groupName:
                return True
        return False
    except:
        logger.warning("Finding group failed")
        return False
