#! usr/bin/python #coding=utf-8
# Author: Peiren Yang
# 20.10.2017
# Download examination files for Rechnerstruktur am KIT
# On linux, windows should change the path_pre

from selenium import webdriver
import time
import requests
import os

drive = webdriver.PhantomJS(executable_path='/usr/lib/phantomjs-2.1.1-linux-x86_64/bin/phantomjs')
drive.set_page_load_timeout(100)

drive.get('https://capp.itec.kit.edu/teaching/rs/index.php?sem=ss17&lang=d')
time.sleep(5)

path_pre = '/home/peiren/Documents/SS2017/Rechnerstruktur/Klasuren/'


def get_exam():
    items = drive.find_elements_by_xpath('//*[@id="content"]/div/div[8]/table/tbody/tr/td/table/tbody/tr')
    counter_year = 0
    counter_file = 0
    for item in items[:-2]:
        tds = item.find_elements_by_xpath('td')
        if len(tds) > 0:
            abs_path = tds[1].find_element_by_tag_name('a').get_attribute('href')
            folder_name = abs_path.split('/')[-3]
            counter_year += 1
            os.mkdir(path_pre + folder_name)
            for td in tds[1:]:
                abs_path = td.find_element_by_tag_name('a').get_attribute('href')
                file_name = abs_path.split('/')[-1]
                path = path_pre + folder_name + '/' + file_name
                file = requests.get(abs_path, stream=True)
                with open(path, 'wb') as f:
                    counter_file += 1
                    for chunk in file.iter_content(chunk_size=1024):
                        if chunk:
                            f.write(chunk)
                print('file downloaded from ' + abs_path)
                print('file stored in ' + path)
                time.sleep(5)
    print('Totally' + counter_file + ' files in ' + counter_year + ' years were downloaded!')


if __name__ == '__main__':
    get_exam()
