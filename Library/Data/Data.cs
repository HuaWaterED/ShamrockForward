using System;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace Shamrock_Forward.Library.Data
{
    public class Data//收
    {
        public long user_id;
        public string nickname = string.Empty;
        public string user_name = string.Empty;
        public string user_displayname = string.Empty;
        public string user_remark = string.Empty;
        public long age;
        public string sex = string.Empty;
        public string title = string.Empty;
        public long title_expire_time;
        public long distance;
        public List<long> honor = new();
        public long join_time;
        public long last_active_time;
        public long last_sent_time;
        public string unique_name = string.Empty;
        public string area = string.Empty;
        public string level = string.Empty;
        public string role = string.Empty;
        public bool unfriendly;
        public bool card_changeable;
        public long gender;
        public long group_id;
        public string platform = string.Empty;
        public string term_type = string.Empty;
        public string source = string.Empty;
        public string group_name = string.Empty;
        public string group_remark = string.Empty;
        public long group_uin;
        public List<long> admins = new();
        public string class_text = string.Empty;
        public bool is_frozen;
        public long max_member;
        public long max_member_count;
        public long member_num;
        public long member_count;
        public long sender_id;
        public string sender_nick = string.Empty;
        public long sender_name;
        public long operator_id;
        public string operator_nick = string.Empty;
        public long operator_time;
        public int message_id;
        public bool @is;
        public long time;
        public string message_type = string.Empty;
        public int real_id;
        public Sender sender = new();
        public object message;
        public long target_id;
        public long peer_id;
        public List<Message> messages = new();
        public List<HonorInfo> current_talkative = new();
        public List<HonorInfo> talkative_list = new();
        public List<HonorInfo> performer_list = new();
        public List<HonorInfo> legend_list = new();
        public List<HonorInfo> strong_newbie_list = new();
        public List<HonorInfo> emotion_list = new();
        public List<HonorInfo> all = new();
        public List<InvitedRequest> invited_requests = new();
        public List<JoinRequest> join_requests = new();

        public List<ModelDetail> variants = new();
        public OnlineClients clients = new();
    }
    #region 收
    public class Message
    {
        public int time;
        public string message_type = string.Empty;
        public int message_id;
        public int real_id;
        public Sender sender = new();
        public object message;
        public int group_id;
        public int target_id;
        public int peer_id;
    }
    public class Sender
    {
        public int user_id;
        public string nickname = string.Empty;
        public string sex = string.Empty;
        public int age;
        public string uid = string.Empty;
    }
    public class InvitedRequest
    {
        public int request_id;
        public int invitor_uin;
        public string invitor_nick = string.Empty;
        public int group_id;
        public string group_name = string.Empty;
        public bool @checked;
        public int actor;
        public int requester_uin;
        public string flag = string.Empty;
    }
    public class JoinRequest
    {
        public int request_id;
        public int requester_uin;
        public string requester_nick = string.Empty;
        public string message = string.Empty;
        public int group_id;
        public string group_name = string.Empty;
        public bool @checked;
        public int actor;
        public string flag = string.Empty;
    }
    public class HonorInfo
    {
        public int user_id;
        public string nickname = string.Empty;
        public string avatar = string.Empty;
        public int day_count;
        public int id;
        public string description = string.Empty;
    }
    public class ModelDetail
    {
        public string model_show = string.Empty;
        public bool need_pay;
    }
    public class OnlineClients
    {
        public int app_id;
        public string device_name = string.Empty;
        public string device_kind = string.Empty;
    }
    #endregion
    public enum Action//发
    {
        get_login_info,
        get_status,
        get_version_info,
        set_qq_profile,
        _get_model_show,
        _set_model_show,
        /// <summary>
        /// 未实现
        /// </summary>
        get_online_clients,
        get_stranger_info,
        get_friend_list,
        get_guild_service_profile,
        /// <summary>
        /// 未实现
        /// </summary>
        get_unidirectional_friend_list,
        get_group_info,
        get_group_list,
        get_group_member_info,
        get_group_member_list,
        get_group_honor_info,
        get_group_system_msg,
        get_essence_msg_list,
        is_blacklist_uin,
        delete_friend,
        delete_unidirectional_friend,
        delete_msg,
        send_private_msg,//
        send_group_msg,//
        send_msg,//
        get_msg,
        get_history_msg,
        get_group_msg_history,
        clear_msgs,//从这里开始没整收发参数了
        get_forward_msg,
        send_group_forward_msg,//
        send_private_forward_msg,//
        get_image,
        can_send_image,
        ocr_image,
        get_record,
        can_send_record,
        //get_record
        //获取文件
        //获取视频
        //获取缩略图
        set_friend_add_request,//
        set_group_add_request,//
        set_group_name,
        set_group_portrait,
        set_group_admin,
        set_group_card,
        set_group_special_title,
        set_group_ban,
        set_group_whole_ban,
        set_essence_msg,
        delete_essence_msg,
        send_group_sign,
        _send_group_notice,
        _get_group_notice,
        set_group_kick,
        set_group_leave,
        group_touch,//
        get_prohibited_member_list,
        upload_private_file,
        upload_group_file,
        delete_group_file,
        create_group_file_folder,
        delete_group_folder,
        get_group_file_system_info,
        get_group_root_files,
        get_group_files_by_folder,
        get_group_file_url,
        switch_account,
        upload_file,
        download_file,
        get_device_battery,
        get_start_time,
        log,
        shut,
        get_weather_city_code,
        get_weather,
        upload_group_image
    }
    public class Params//发
    {
        public long user_id;
        public string message = string.Empty;
        public string nickname = string.Empty;
        public string company = string.Empty;
        public string email = string.Empty;
        public string college = string.Empty;
        public string personal_note = string.Empty;
        public int age;
        public string birthday = string.Empty;
        public string model = string.Empty;
        public string manu = string.Empty;
        public bool no_cache;
        public long group_id;
        public int message_id;
        public bool auto_escape;
        public string message_type = string.Empty;
        public int discuss_id;
        public int count;
        public int message_seq;
        public string id = string.Empty;
        public Ext ext = new();
    }
    #region 发
    public class Ext
    {
        public int add_src_id;
        public string add_src_name = string.Empty;
        public int add_sub_src_id;
        public bool allow_cal_interactive;
        public bool allow_click;
        public bool allow_people_see;
        public int auth_state;
        public int big_club_vip_open;
        public int hollywood_vip_open;
        public int qq_vip_open;
        public int super_qq_open;
        public int super_vip_open;
        public int voted;
        public bool baby_q_switch;
        public string bind_phone_info = string.Empty;
        public int card_id;
        public int card_type;
        public int category;
        public int clothes_id;
        public string cover_url = string.Empty;
        public object declaration = string.Empty;
        public int default_card_id;
        public object diy_complicated_info = string.Empty;
        public object diy_default_text = string.Empty;
        public object diy_text = string.Empty;
        public float diy_text_degree;
        public int diy_text_font_id;
        public float diy_text_height;
        public float diy_text_width;
        public float diy_text_loc_x;
        public float diy_text_loc_y;
        public bool dress_up_is_on;
        public object enc_id = string.Empty;
        public int enlarge_qzone_pic;
        public int extend_friend_entry_add_friend;
        public int extend_friend_entry_contact;
        public int extend_friend_flag;
        public int extend_friend_question;
        public int extend_friend_voice_duration;
        public int favorite_source;
        public int feed_preview_time;
        public int font_id;
        public int font_type;
        public string qid_bg_url = string.Empty;
        public string qid_color = string.Empty;
        public string qid_logo_url = string.Empty;
        public bool qq_card_is_on;
        public object school_id = string.Empty;
        public object school_name = string.Empty;
        public bool school_verified_flag;
        public bool show_publish_button;
        public string singer = string.Empty;
        public int song_dura;
        public string song_id = string.Empty;
        public string song_name = string.Empty;
    }
    #endregion
}