package com.sihus.core.util;

import java.io.BufferedReader;
import java.io.BufferedWriter;
import java.io.File;
import java.io.FileInputStream;
import java.io.FileReader;
import java.io.FileWriter;
import java.io.InputStreamReader;

public class FileUtil {

public static void main(String[] args) {
      write("F:\\123.txt", "hello");
      read("F:\\123.txt");
}

public static void write(String path, String content) {
      String s = new String();
      String s1 = new String();
      try {
       File f = new File(path);
       if (f.exists()) {
//        System.out.println("文件存在");
       } else {
        System.out.println("文件不存在，正在创建...");
        if (f.createNewFile()) {
         System.out.println("文件创建成功！");
        } else {
         System.out.println("文件创建失败！");
        }

       }
       BufferedReader input = new BufferedReader(new FileReader(f));

       while ((s = input.readLine()) != null) {
        s1 += s + "\n";
       }
//       System.out.println("文件内容：" + s1);
       input.close();
       s1 += content;

       BufferedWriter output = new BufferedWriter(new FileWriter(f));
       output.write(s1);
       output.close();
      } catch (Exception e) {
       e.printStackTrace();
      }
}




///**
//   * @param args
//   */
//public static void main(String[] args) {
//   read("E:\\123.txt");
//}

public static void read(String file) {
   String s = null;
   StringBuffer sb = new StringBuffer();
   File f = new File(file);
   if (f.exists()) {
    System.out.println("文件存在");

    try {
     BufferedReader br = new BufferedReader(new InputStreamReader(
       new FileInputStream(f)));

     while ((s = br.readLine()) != null) {
      sb.append(s);
     }
     System.out.println(sb);
    } catch (Exception e) {
     e.printStackTrace();
    }
   }else{
    System.out.println("文件不存在!");
   }
}
}
