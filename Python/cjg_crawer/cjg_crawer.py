#! usr/bin/python #coding=utf-8

from selenium import webdriver
import time
import re
import pymysql

drive = webdriver.PhantomJS(executable_path='/usr/lib/phantomjs-2.1.1-linux-x86_64/bin/phantomjs')
# set timeout interval to 100 s, the 220th topic has problem with page loading.
drive.set_page_load_timeout(100)


class DbOperation(object):
    def __init__(self):
        self.db = pymysql.connect(host='localhost', user='root', passwd='19901217',
                                  db='crawer_data', charset='utf8')
        self.cs = self.db.cursor()

    def insert(self, sql, paras=None):
        return self.cs.execute(sql, paras)

    def inquire(self, sql):
        self.cs.execute(sql)
        return self.cs.fetchall()

    def close(self):
        self.cs.close()
        self.db.close()

    def my_insert(self, paras=None):
        sql = 'insert into topic (title, reply_counter, poster_name, view_counter, last_time, link)' \
              ' values(%s, %s, %s, %s, %s, %s)'
        if self.insert(sql, paras) == 0:
            raise Exception('Execution failed!')

            # return the ID if needed
            # return self.inquire('select last_insert_id()')[0][0]

    def post_insert(self, paras=None):
        sql = 'insert into content (topic_id, post_time, poster_name, content)' \
              ' values(%s, %s, %s, %s)'
        if self.insert(sql, paras) == 0:
            raise Exception('Execution failed!')


db_op = DbOperation()


def login():
    url = 'http://kaforum.de'
    drive.get(url)
    time.sleep(5)
    drive.find_element_by_xpath('//*[@id="cover_mainmenu"]/div[1]/ul/li[1]/a').click()
    time.sleep(5)

    if re.search(r'登入', drive.title) is not None:
        time.sleep(5)
        drive.find_element_by_name('username').send_keys('peilovepei')
        drive.find_element_by_name('password').send_keys('19901217')
        drive.find_element_by_name('login').click()
        time.sleep(5)
        if re.search(r'登入', drive.title) is not None:
            return 'login failed!'

    return 'logged in!'


def get_topic():
    drive.get('http://kaforum.de/forum/viewforum.php?f=20')
    time.sleep(5)

    for counter in range(30, 961, 30):
        items = drive.find_elements_by_xpath('/html/body/table[5]/tbody/tr/td/table[1]/tbody/tr')
        for i in range(len(items)):
            paras = items[i].find_elements_by_tag_name('td')
            if len(paras) < 7:
                continue
            data = [paras[2].text, paras[3].text, paras[4].text, paras[5].text,
                    paras[6].text, paras[2].find_elements_by_tag_name('a')[0].get_attribute('href')]
            try:
                db_op.my_insert(data)
            except Exception as ex:
                print(ex.args)

        db_op.db.commit()
        drive.get('http://kaforum.de/forum/viewforum.php?f=20&topicdays=0&start=' + str(counter))
        time.sleep(5)

    print(str(db_op.inquire('select count(*) from topic')[0][0]) + ' topics are crawled')


def get_post():
    # Now start crawling the posts
    urls = db_op.inquire('select id, reply_counter, link from topic')
    for address in urls:
        try:
            # open link
            print('getting url ' + address[2])
            print('the ' + str(address[0]) + 'th topic with ' + str(address[1]) + ' replies')
            drive.get(address[2])
            time.sleep(5)
            # get these 4 values, one from last update, the other three from webpage
            # data[1] is needed to control the loop for insertion and further page request
            # /html/body/table[5]/tbody/tr/td/table[3]

            posts = drive.find_elements_by_xpath(
                '/html/body/table[5]/tbody/tr/td/table[3]/tbody/tr')
            for cq in range(min(int(address[1]) + 1, 15)):
                text = 'useless info'
                if len(posts[4 * cq + 1].find_element_by_class_name('postbody').text) < 1000:
                    text = posts[4 * cq + 1].find_element_by_class_name('postbody').text
                db_op.post_insert([address[0], ' '.join(
                    (posts[4 * cq + 1].find_elements_by_class_name('postdetails'))[1].text.split(' ')[1:7]),
                                   posts[4 * cq + 1].find_element_by_class_name('name').text, text])
            db_op.db.commit()

            if int(address[1]) > 15:
                for l_time in range(1, int(int(address[1]) / 15) + 1):
                    drive.get('http://kaforum.de/forum/viewtopic.php?t=23742&start=' + str(l_time * 15))
                    time.sleep(5)
                    posts = drive.find_elements_by_xpath(
                        '/html/body/table[5]/tbody/tr/td/table[3]/tbody/tr')

                    if int(address[1]) - l_time * 15 > 15:
                        n_time = 15
                    else:
                        n_time = int(address[1]) % 15 + 1

                    for cq in range(n_time):
                        text = 'useless info'
                        if len(posts[4 * cq + 1].find_element_by_class_name('postbody').text) < 1000:
                            text = posts[4 * cq + 1].find_element_by_class_name('postbody').text
                        db_op.post_insert([address[0], ' '.join(
                            (posts[4 * cq + 1].find_elements_by_class_name('postdetails'))[1].text.split(' ')[1:7]),
                                           posts[4 * cq + 1].find_element_by_class_name('name').text, text])
                db_op.db.commit()
            total_num = str(db_op.inquire('select count(*) from content where topic_id = ' + str(address[0]))[0][0])
            print('Total ' + total_num + ' posts inserted.')
        except Exception as ec:
            print(ec.args)
    # print(str(db_op.inquire('select count(*) from content')[0][0]) + ' posts are crawled')
    db_op.close()


print(login())
# get_topic()
get_post()


# every page of this forum contains 30 topics, start with 0 end with 960 at page 33, but page
# 33 is not full.

# The structure of the table
# first 2 and last <tr> contain no useful info

# Structure of <tr>
# contains lots of <td>
# The xpath of the first useful <td>
# /html/body/table[5]/tbody/tr/td/table[1]/tbody/tr[3]/td[3]

# first span in td[3] -- class="topictitle"
# /html/body/table[5]/tbody/tr/td/table[1]/tbody/tr[3]/td[3]/span[1]/a

# td[4] -- class="postditails" i.e. how many replies, info for clawing into the topic

# td[5] -- class="name", the name of the poster

# td[6] -- class="postdetails", view counter

# td[7] -- class="postdetails", time of the last post
